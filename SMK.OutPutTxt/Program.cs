using Microsoft.Extensions.Configuration;
using SMK.OutPutTxt.Main;

class Program
{
    static void Main(string[] args)
    {
        //查詢組態檔案
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        string connectionString = config.GetConnectionString("db");

        //三個參數的Key都要一樣，不然會出現排序問題
        //他是config.GetSection("Query").GetChildren().OrderBy(s => s.Key) -->  也就是英文排序

        //查詢確認是否執行 (True or False)
        Dictionary<string, string> checkRuns = config.GetSection("CheckRun").GetChildren()
                                                   .ToDictionary(section => section.Key, section => section.Value);
        //查詢T-SQL
        Dictionary<string, string> queries = config.GetSection("Query").GetChildren()
                                                   .ToDictionary(section => section.Key, section => section.Value);

        //查詢路徑
        Dictionary<string, string> rootPaths = config.GetSection("FileRootPath").GetChildren()
                                                          .ToDictionary(section => section.Key, section => section.Value);

        //查詢檔案名稱
        Dictionary<string, string> fileNames = config.GetSection("FileName").GetChildren()
                                                         .ToDictionary(section => section.Key, section => section.Value);

        //匯出檔案
        //傳入方法的參數(連接字串,T-SQL,存檔路徑,檔案名稱)
        OutPutService outPutService = new OutPutService();
        foreach (string key in queries.Keys)
        {
            if (checkRuns[key] != "True") continue;
            outPutService.Output_Txt(connectionString, queries[key], rootPaths[key], fileNames[key]);
        }

        //下列為參考的code

        //二代

        //三個參數的Key都要一樣，不然會出現排序問題
        //他是config.GetSection("Query").GetChildren().OrderBy(s => s.Key) -->  也就是英文排序

        ////查詢T-SQL
        //List<string> Query = config.GetSection("Query").GetChildren()
        //                       .Select(section => section.Value)
        //                       .ToList();

        ////查詢路徑
        //List<string> RootPath = config.GetSection("FileRootPath").GetChildren()
        //                                     .Select(section => section.Value)
        //                                     .ToList();

        ////查詢檔案名稱
        //List<string> FileName = config.GetSection("FileName").GetChildren()
        //                                       .Select(section => section.Value)
        //                                       .ToList();

        ////匯出檔案
        ////傳入方法的參數(連接字串,T-SQL,存檔路徑,檔案名稱)
        //OutPutService outPutService = new OutPutService();
        //for (int i = 0; i < Query.Count; i++)
        //{
        //    outPutService.Output_Txt(connectionString, Query[i], RootPath[i], FileName[i]);
        //}

        //List<string> Query = new List<string>();
        //foreach (IConfigurationSection section in config.GetSection("Query").GetChildren())
        //{
        //    Query.Add(section.Value);
        //}     
        //List<string> RootPath = new List<string>();
        //foreach (IConfigurationSection section in config.GetSection("FileService").GetChildren())
        //{
        //    RootPath.Add(section.Value);
        //}      
        //List<string> FileName = new List<string>();
        //foreach (IConfigurationSection section in config.GetSection("FileName").GetChildren())
        //{
        //    FileName.Add(section.Value);
        //}

        //初代

        //string IniDrDtlRootPath = config.GetSection("FileService")["IniDrDtlRootPath"];
        //string IniDrOrdRootPath = config.GetSection("FileService")["IniDrOrdRootPath"];
        //string IniOpDtlRootPath = config.GetSection("FileService")["IniOpDtlRootPath"];
        //string IniOpOrdRootPath = config.GetSection("FileService")["IniOpOrdRootPath"];
        //string ICCardDataRootPath = config.GetSection("FileService")["ICCardDataRootPath"];
        //string HospContractRootPath = config.GetSection("FileService")["HospContractRootPath"];

        //string iniDrDtl_FileName = config.GetSection("FileName")["IniDrDtl"];
        //string iniDrOrd_FileName = config.GetSection("FileName")["IniDrOrd"];
        //string iniOpDtl_FileName = config.GetSection("FileName")["IniOpDtl"];
        //string iniOpOrd_FileName = config.GetSection("FileName")["IniOpOrd"];
        //string ICCardData_FileName = config.GetSection("FileName")["ICCardData"];
        //string HospContract_FileName = config.GetSection("FileName")["HospContract"];

        //string iniDrDtl_Query = config.GetSection("Query")["IniDrDtl"];
        //string iniDrOrd_Query = config.GetSection("Query")["IniDrOrd"];
        //string iniOpDtl_Query = config.GetSection("Query")["IniOpDtl"];
        //string iniOpOrd_Query = config.GetSection("Query")["IniOpOrd"];
        //string iCCardData_Query = config.GetSection("Query")["ICCardData"];
        //string HospContract_Query = config.GetSection("Query")["HospContract"];

        //outPutService.Output_Txt(connectionString, HospContract_Query, HospContractRootPath, HospContract_FileName);
        //outPutService.Output_Txt(connectionString, iCCardData_Query, ICCardDataRootPath, ICCardData_FileName);
        //outPutService.Output_Txt(connectionString, iniDrDtl_Query, IniDrDtlRootPath, iniDrDtl_FileName);
        //outPutService.Output_Txt(connectionString, iniDrOrd_Query, IniDrOrdRootPath, iniDrOrd_FileName);
        //outPutService.Output_Txt(connectionString, iniOpDtl_Query, IniOpDtlRootPath, iniOpDtl_FileName);
        //outPutService.Output_Txt(connectionString, iniOpOrd_Query, IniOpOrdRootPath, iniOpOrd_FileName);

        //string SftpHost = config.GetSection("SFTP")["IP"];
        //int SftpPort = Int16.Parse(config.GetSection("SFTP")["port"]);
        //string Sftpusername = config.GetSection("SFTP")["Account"];
        //string Sftppassword = config.GetSection("SFTP")["Password"];
        //string SftpremotePath = config.GetSection("SFTP")["Path"];
        //SftpHelper sftpHelper = new SftpHelper();
        //sftpHelper.UploadFile(SftpHost, SftpPort, Sftpusername, Sftppassword, SftpremotePath, IniDrDtlRootPath + "/" + iniDrDtl);
    }
}
