using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMK.Shared.Utilities;

namespace SMK.Worker.Extension
{
    public static class DbContextExtension
    {
        public static int CountByRawSql(this DbContext dbContext, string sql, params KeyValuePair<string, object>[] parameters)
        {
            int result = -1;
            SqlConnection connection = dbContext.Database.GetDbConnection() as SqlConnection;

            try
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;

                    foreach (KeyValuePair<string, object> parameter in parameters)
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);

                    using (DbDataReader dataReader = command.ExecuteReader())
                        if (dataReader.HasRows)
                            while (dataReader.Read())
                                result = dataReader.GetInt32(0);
                }
            }
            finally { connection.Close(); }

            return result;
        }

        public static string CreateTempTable(this DbContext dbContext, string tableName)
        {
            var text = File.ReadAllText($@".\\Templates\\{tableName}.sql.tpl");
            var random = KeyGenerator.GetUniqueKey(5);
            var tempTableName = tableName + "_" + random;
            var sql = string.Format(text, tempTableName);
            dbContext.Database.ExecuteSqlRaw(sql);
            return tempTableName;
        }

        public static void DropTable(this DbContext dbContext, string tableName)
        {
            var sql = string.Format("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)", tableName);
            sql += string.Format("  drop table [dbo].[{0}]", tableName);
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public static void DropTable(this DbContext dbContext, string tableName, DbConnection connection)
        {
            var sql = string.Format("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)", tableName);
            sql += string.Format("  drop table [dbo].[{0}]", tableName);
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public static void RenameTable(this DbContext dbContext, string oldTableName, string newTableName)
        {
            var sql = string.Format("sp_rename {0}, {1}", oldTableName, newTableName);
            dbContext.Database.ExecuteSqlRaw(sql);
        }
        public static void RenameTable(this DbContext dbContext, string oldTableName, string newTableName, DbConnection connection)
        {
            var sql = string.Format("sp_rename {0}, {1}", oldTableName, newTableName);
            dbContext.Database.ExecuteSqlRaw(sql);
        }

        public static bool TableIsNotEmpty(this DbContext dbContext, string tableName)
        {
            var sql = string.Format("select count(1) from {0}", tableName);
            var count = dbContext.CountByRawSql(sql, null);
            return count > 0;
        }

        public static IEnumerable<DbColumnInfo> GetDbColumns(this DbContext dbContext, Type clrEntityType)
        {
            var entityType = dbContext.Model.FindEntityType(clrEntityType);

            var columns = new List<DbColumnInfo>();
            return entityType.GetProperties()
                .Select(x => new DbColumnInfo
                {
                    ClrName = x.Name,
                    Name = x.GetColumnName(),
                    Type = x.ClrType.FullName,
                });
        }

        public static string GetTableName(this DbContext dbContext, Type clrEntityType)
        {
            var entityType = dbContext.Model.FindEntityType(clrEntityType);
            return entityType.GetTableName();
        }

        public static void BulkCopy<T>(this DbContext dbContext, IEnumerable<T> entities, DbConnection connection)
        {
            var columns = GetDbColumns(dbContext, typeof(T));
            var tableName = GetTableName(dbContext, typeof(T));
            var dataTable = new DataTable(tableName);
            var dbColumnInfos = columns as DbColumnInfo[] ?? columns.ToArray();
            dataTable.Columns.AddRange(dbColumnInfos.Select((x => new DataColumn()
            {
                ColumnName = x.Name,
                DataType = System.Type.GetType(x.Type)
            })).ToArray());
            foreach (var entity in entities)
            {
                var row = dataTable.NewRow();
                foreach (var c in dbColumnInfos)
                {
                    var props = TypeDescriptor.GetProperties(entity);
                    var prop = props[c.ClrName];
                    row[c.Name] = prop.GetValue(entity);
                }
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();

            using var bulkCopy = new SqlBulkCopy((SqlConnection)connection);
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.WriteToServer(dataTable);
        }

        public static void BulkCopy<T>(this DbContext dbContext, string tableName, IEnumerable<T> entities)
        {
            var columns = GetDbColumns(dbContext, typeof(T));
            var dataTable = new DataTable(tableName);
            var dbColumnInfos = columns as DbColumnInfo[] ?? columns.ToArray();
            dataTable.Columns.AddRange(dbColumnInfos.Select((x => new DataColumn()
            {
                ColumnName = x.Name,
                DataType = System.Type.GetType(x.Type)
            })).ToArray());

            foreach (var entity in entities)
            {
                var row = dataTable.NewRow();
                foreach (var c in dbColumnInfos)
                {
                    var props = TypeDescriptor.GetProperties(entity);
                    var prop = props[c.ClrName];
                    row[c.Name] = prop.GetValue(entity);
                }
                dataTable.Rows.Add(row);
            }
            dataTable.AcceptChanges();

            var connection = dbContext.Database.GetDbConnection();
            if (connection.State == ConnectionState.Broken ||
                connection.State == ConnectionState.Closed)
                connection.Open();

            using var bulkCopy = new SqlBulkCopy((SqlConnection)connection);
            bulkCopy.BulkCopyTimeout = 200000;
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.WriteToServer(dataTable);
        }
    }
}
