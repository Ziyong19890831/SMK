using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SMK.Data.Entity;
using SMK.Data.Enums;
using SMK.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yozian.Extension;

namespace SMK.Data
{
    public partial class SMKWEBContext : DbContext
    {
        public override int SaveChanges()
        {

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public int SaveChangesWithAudit(string account, string actionRemark = "")
        {
            handleAudit(account, actionRemark);
            return base.SaveChanges();
        }

        public Task<int> SaveChangesWithAuditAsync(string account, string actionRemark = "")
        {
            handleAudit(account, actionRemark);
            return this.SaveChangesAsync();
        }


        public IDbContextTransaction GetTransactionScope()
        {
            return this.Database.BeginTransaction();
        }

        public void UseTransaction(IDbContextTransaction transaction)
        {
            this.Database.UseTransaction(transaction as DbTransaction);
        }


        private void handleAudit(string account, string actionRemark)
        {
            var auditRecords = new List<AuditLog>();
            var auditStateList = new[] {
                EntityState.Added ,
                EntityState.Modified,
                EntityState.Deleted
            };
            this.ChangeTracker
                 .Entries()
                 .ForEach(entry =>
                 {
                     var et = this.Model.FindEntityType(entry.Entity.GetType());
                     if (et == null)
                     {
                         //處理viewmodel繼承entity class
                         et = this.Model.FindEntityType(entry.Entity.GetType().BaseType);
                     }
                     var propNames = entry.Metadata.GetProperties().Select(p => p.Name);
                     var modifiedProperties = propNames
                             .Select(n => entry.Property(n))
                             .Where(e => e.IsModified)
                             .Select(e => e.Metadata.Name);

                     var pk = et.FindPrimaryKey().Properties.Select(p => p.Name);
                     var pkValue = entry
                            .Properties
                            .Where(x => pk.Any(q=>q.Equals(x.Metadata.Name)))
                            .Select(x=>x.CurrentValue);

                     var audit = new AuditLog()
                     {
                         Id = MyGuid.NewGuid(),
                         SourceTable = et.GetTableName(),
                         Account = account,
                         RecordId =string.Join(",",pkValue),
                         OriginalRecord = JsonConvert.SerializeObject(entry.OriginalValues.ToObject()),
                         CurrentRecord = JsonConvert.SerializeObject(entry.Entity),
                         InvolvedColumns = string.Join(",", modifiedProperties),
                         ActionRemark = actionRemark
                     };

                     switch (entry.State)
                     {
                         case EntityState.Added:
                             audit.ActionType = AuditActionType.Create;
                             break;
                         case EntityState.Modified:
                             audit.ActionType = AuditActionType.Update;
                             break;
                         case EntityState.Deleted:
                             audit.ActionType = AuditActionType.Remove;
                             break;
                     }
                     // add only what we want to tracked

                     if (auditStateList.Contains(entry.State))
                     {
                         auditRecords.Add(audit);
                     }
                 });

            this.AddRange(auditRecords);
        }
        public IEnumerable<dynamic> GetDynamicResult(string commandText, params SqlParameter[] parameters)
        {
            // Get the connection from DbContext
            var connection = this.Database.GetDbConnection();

            // Open the connection if isn't open
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.Connection = connection;
                command.CommandTimeout = 300000;
                if (parameters?.Length > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                using (var dataReader = command.ExecuteReader())
                {
                    // List for column names
                    var names = new List<string>();

                    if (dataReader.HasRows)
                    {
                        // Add column names to list
                        for (var i = 0; i < dataReader.VisibleFieldCount; i++)
                        {
                            names.Add(dataReader.GetName(i));
                        }

                        while (dataReader.Read())
                        {
                            // Create the dynamic result for each row
                            var result = new ExpandoObject() as IDictionary<string, object>;

                            foreach (var name in names)
                            {
                                // Add key-value pair
                                // key = column name
                                // value = column value
                                result.Add(name, dataReader[name]);
                            }

                            yield return result;
                        }
                    }
                }
            }
        }
    }
}
