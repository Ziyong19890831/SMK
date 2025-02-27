using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ICCardConsole
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                Utility.CreateFolder();
                Console.WriteLine("系統執行中，請勿關閉.....");
                iniDrDtlFileSystemWatcher();
                iniDrOrdFileSystemWatcher();
                iniOpDtlFileSystemWatcher();
                iniOpOrdFileSystemWatcher();
                AgentPatientFileSystemWatcher();
                QsCureFileSystemWatcher();
                QsDataFileSystemWatcher();
                QsStateFileSystemWatcher();
                ICCardFileSystemWatcher();
                SamplingListFileSystemWatcher();
                HospBscAllFileSystemWatcher();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            
            Console.ReadKey();
        }
        public static void ICCard()
        {
            string[] ICCardfiles = Directory.GetFiles("ICCardTxt");
            if (ICCardfiles.Length != 0)
            {
                Utility.DeleteICCard();
                int CompliteCount = 0;
                foreach (string file in ICCardfiles)
                {
                    CompliteCount = Utility.OpenICCardTxt(file);
                    Console.WriteLine("已完成ICCard" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除ICCard" + file);
                }
                Utility.InsertLog("ICCardfiles", CompliteCount);
            }
        }
        public static void AgentPatient()
        {
            string[] AgentPatientTxt = Directory.GetFiles("AgentPatientTxt");
            if (AgentPatientTxt.Length != 0)
            {
                Utility.DeleteAgentPatientTxt();
                int CompliteCount = 0;
                foreach (string file in AgentPatientTxt)
                {
                    CompliteCount = Utility.OpeAgentPatientTxt(file);
                    Console.WriteLine("已完成AgentPatient" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除AgentPatient" + file);
                }
                Utility.InsertLog("AgentPatientTxt", CompliteCount);
            }
        }
        public static void QSCure()
        {
            string[] QSCureTxt = Directory.GetFiles("QSCureTxt");
            if (QSCureTxt.Length != 0)
            {
                Utility.DeleteQSCureTxt();
                int CompliteCount = 0;
                foreach (string file in QSCureTxt)
                {
                    CompliteCount = Utility.OpeQSCureTxt(file);
                    Console.WriteLine("已完成QSCure" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除QSCure" + file);
                }
                Utility.InsertLog("QSCureTxt", CompliteCount);
            }
        }
        public static void QSData()
        {
            string[] QSDataTxt = Directory.GetFiles("QSDataTxt");
            if (QSDataTxt.Length != 0)
            {
                Utility.DeleteQsData();
                int CompliteCount = 0;
                foreach (string file in QSDataTxt)
                {
                    CompliteCount = Utility.OpeQSDataTxt(file);
                    Console.WriteLine("已完成QSData" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除QSData" + file);
                }
                Utility.InsertLog("QSData", CompliteCount);
            }
        }
        public static void QSState()
        {
            string[] QSStateTxt = Directory.GetFiles("QSStateTxt");
            if (QSStateTxt.Length != 0)
            {
                Utility.DeleteQsState();
                int CompliteCount = 0;
                foreach (string file in QSStateTxt)
                {
                    CompliteCount = Utility.OpeQSStateTxt(file);
                    Console.WriteLine("已完成QSState" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除QSState" + file);
                }
                Utility.InsertLog("QSState", CompliteCount);
            }
        }
        public static void iniDrDtl()
        {
            string[] iniDrDtlTxt = Directory.GetFiles("iniDrDtlTxt");
            if (iniDrDtlTxt.Length != 0)
            {
                int CompliteCount = 0;
                foreach (string file in iniDrDtlTxt)
                {
                    CompliteCount = Utility.OpeniniDrDtlTxt(file);
                    Console.WriteLine("已完成iniDrDtl" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除iniDrDtl" + file);
                }
                Utility.InsertLog("iniDrDtl", CompliteCount);
            }
        }
        public static void iniDrOrd()
        {
            string[] iniDrOrdTxt = Directory.GetFiles("iniDrOrdTxt");
            if (iniDrOrdTxt.Length != 0)
            {
                int CompliteCount = 0;
                foreach (string file in iniDrOrdTxt)
                {
                    CompliteCount = Utility.OpeniniDrOrdTxt(file);
                    Console.WriteLine("已完成iniDrOrd" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除iniDrOrd" + file);
                }
                Utility.InsertLog("iniDrOrd", CompliteCount);
            }
        }
        public static void iniOpDtl()
        {
            string[] iniOpDtlTxt = Directory.GetFiles("iniOpDtlTxt");
            if (iniOpDtlTxt.Length != 0)
            {
                int CompliteCount = 0;
                foreach (string file in iniOpDtlTxt)
                {
                    CompliteCount = Utility.OpeniniOpDtlTxt(file);
                    Console.WriteLine("已完成iniOpDtl" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除iniOpDtl" + file);
                }
                Utility.InsertLog("iniOpDtl", CompliteCount);
            }
        }
        public static void iniOpOrd()
        {
            string[] iniOpOrdTxt = Directory.GetFiles("iniOpOrdTxt");
            if (iniOpOrdTxt.Length != 0)
            {
                int CompliteCount = 0;
                foreach (string file in iniOpOrdTxt)
                {
                    CompliteCount = Utility.OpeniniOpOrdTxt(file);
                    Console.WriteLine("已完成iniOpOrd" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除iniOpOrd" + file);
                }
                Utility.InsertLog("iniOpOrd", CompliteCount);
            }
        }
        public static void SamplingList()
        {
            string[] SamplingListTxt = Directory.GetFiles("SamplingListTxt");
            if (SamplingListTxt.Length != 0)
            {
                int CompliteCount = 0;
                foreach (string file in SamplingListTxt)
                {
                    CompliteCount = Utility.OpenSamplingListTxt(file);
                    Console.WriteLine("已完成SamplingList" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除SamplingList" + file);
                }
                Utility.InsertLog("SamplingList", CompliteCount);
            }
        }
        public static void HospBscAll()
        {
            string[] HospBscAllTxt = Directory.GetFiles("HospBscAllTxt");
            if (HospBscAllTxt.Length != 0)
            {
                Utility.DeleteHospBscAllTxt();
                int CompliteCount = 0;
                foreach (string file in HospBscAllTxt)
                {
                    CompliteCount = Utility.OpenHospBscAllTxt(file);
                    Console.WriteLine("已完成HospBsc" + file + "共完成" + CompliteCount + "筆");
                    File.Delete(file);
                    Console.WriteLine("已刪除HospBsc" + file);
                }
                Utility.InsertLog("HospBsc", CompliteCount);
            }
        }
        #region WatchFile
        private static void iniDrDtlFileSystemWatcher()
        {
           
            FileSystemWatcher iniDrDtlTxt_watch = new FileSystemWatcher();
            iniDrDtlTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["iniDrDtl"];
            iniDrDtlTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrDtlTxt_watch.IncludeSubdirectories = true;
            iniDrDtlTxt_watch.EnableRaisingEvents = true;
            iniDrDtlTxt_watch.Created += new FileSystemEventHandler(iniDrDtl_watch_Created);

        }
        private static void iniDrDtl_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("iniDrDtl執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            iniDrDtl();
        }
        private static void iniDrOrdFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["iniDrOrd"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(iniDrOrd_watch_Created);

        }
        private static void iniDrOrd_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("iniDrOrd執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            iniDrOrd();
        }
        private static void iniOpDtlFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["iniOpDtl"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(iniOpDtl_watch_Created);

        }
        private static void iniOpDtl_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("iniOpDtl執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            iniOpDtl();
        }
        private static void iniOpOrdFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["iniOpOrd"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(iniOpOrd_watch_Created);

        }
        private static void iniOpOrd_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("iniOpOrd執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            iniOpOrd();
        }
        private static void AgentPatientFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["AgentPatient"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(AgentPatient_watch_Created);

        }
        private static void AgentPatient_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("AgentPatient執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            AgentPatient();
        }
        private static void QsCureFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["QsCure"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(QsCure_watch_Created);

        }
        private static void QsCure_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("QSCure執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            QSCure();
        }
        private static void QsDataFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["QsData"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(QsData_watch_Created);

        }
        private static void QsData_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("QSData執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            QSData();
        }
        private static void QsStateFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["QsState"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(QsState_watch_Created);

        }
        private static void QsState_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("QSState執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            QSState();
        }
        private static void ICCardFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["ICCard"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(ICCard_watch_Created);

        }
        private static void ICCard_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.Write("ICCard執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            ICCard();
        }
        private static void SamplingListFileSystemWatcher()
        {
            
            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["SamplingList"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(SamplingList_watch_Created);

        }
        private static void SamplingList_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("SamplingList執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);           
            SamplingList();
        }
        private static void HospBscAllFileSystemWatcher()
        {

            FileSystemWatcher iniDrOrdTxt_watch = new FileSystemWatcher();
            iniDrOrdTxt_watch.Path = System.Configuration.ConfigurationManager.AppSettings["HospBscAll"];
            iniDrOrdTxt_watch.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            iniDrOrdTxt_watch.IncludeSubdirectories = true;
            iniDrOrdTxt_watch.EnableRaisingEvents = true;
            iniDrOrdTxt_watch.Created += new FileSystemEventHandler(HospBscAll_watch_Created);

        }
        private static void HospBscAll_watch_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("HospBscAll執行中.....");
            Console.WriteLine("等待10秒釋放資源.....");
            Thread.Sleep(10000);
            HospBscAll();
        }
        #endregion
    }
}
