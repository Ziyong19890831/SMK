using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using SMK.Data;
using SMK.Shared.Utilities;
using SMK.Worker.Extension;

namespace SMK.Worker.Services
{
    public class DatabaseService
    {
        public ILogger<DatabaseService> Logger { get; set; }
        public IConfiguration Configuration { get; set; }
        public DatabaseService(ILogger<DatabaseService> logger, 
                    IConfiguration configuration,
                    SMKWEBContext context)
        {
            Logger = logger;
            Configuration = configuration;
            Context = context;
        }

        public SMKWEBContext Context { get; set; }

        public string CreateTempTable(string tableName)
        {
            var text = File.ReadAllText($@"..\\{tableName}.sql");
            var random = KeyGenerator.GetUniqueKey(5);
            var tempTableName = @"{tableName}{random}}";
            var sql = string.Format(text, tempTableName);
            Context.Database.ExecuteSqlRaw(sql);
            return tempTableName;
        }

        public void DropTable(string tableName)
        {
            var sql = @"drop table {tableName}";
            Context.Database.ExecuteSqlRaw(sql);
        }
        
        public void RenameTable(string oldTableName, string newTableName)
        {
            var sql = @"sp_rename {oldTableName}, {newTableName}";
            Context.Database.ExecuteSqlRaw(sql);
        }

        public bool TableIsNotEmpty(string tableName)
        {
            var sql = @"select count(1) from {tableName}";
            var count = Context.CountByRawSql(sql, null);
            return count > 0;
        }
    }
}