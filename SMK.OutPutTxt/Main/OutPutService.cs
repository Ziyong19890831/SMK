using Microsoft.Data.SqlClient;
using System.Text;
using static SMK.Data.Enums.ScheduleTxtLogEnums;
using SMK.OutPutTxt.Helpers;

namespace SMK.OutPutTxt.Main
{
    public class OutPutService
    {
        /// <summary>
        /// 輸出txt
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        /// <param name="query">搜尋的sql檔案</param>
        /// <param name="folderPath">上傳檔案的路徑</param>
        /// <param name="fileName">檔案名稱</param>
        public void Output_Txt(string connectionString, string query, string folderPath, string fileName)
        {
            Check_Files check_Files = new Check_Files();
            check_Files.Check_Files_Route(folderPath, fileName);
            int WriteCount = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                InsertLog(connection, fileName);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        string filePath = Path.Combine(folderPath, fileName);  // 組合檔案完整路徑

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        {
                            int fieldCount = reader.FieldCount;
                            // writer.WriteLine("data_id,HospID,fee_ym,ExamYear,ExamTime,FirstTreatDate,WeekCount,InstructExamYear,InstructExamTime,FirstInstructDate,InctructSerial,MedApply,InstructApply,TraceApply,ReleaseApply,appl_type,appl_date,case_type,seq_no,func_type,func_date,rel_date,birthday,id,func_seq_no,pay_type,part_code,icd9cm_code,icd9cm_code1,icd9cm_code2,drug_days,prsn_id,drug_prsn_id,drug_dot,cure_dot,dsvc_code,dsvc_dot,exp_dot,part_amt,appl_dot,orig_hosp_id,Id_Sex,cure_item1,cure_item2,cure_item3,cure_item4,orig_case_type,other_part_amt,appl_cause_mark,icd10cm_code2,icd10cm_code3,icd10cm_code4,corr_hosp_id,area_service,tran_date,name,HospSeqNo,cure_e_date");
                            while (reader.Read())
                            {
                                WriteCount++;
                                //string columnValue = reader.GetString(0);  // 假設欄位索引為0
                                //string columnValue2 = reader.GetString(1);
                                //writer.WriteLine(columnValue+','+ columnValue2);
                                // 使用 StringBuilder 來組合所有欄位的值
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    if (i > 0) sb.Append("|");
                                    string data = reader.IsDBNull(i) ? string.Empty : reader.GetValue(i).ToString().Trim();
                                    sb.Append(data);
                                }

                                // 寫入多欄位資料
                                writer.WriteLine(sb.ToString());
                            }
                        }
                    }
                    UpdataLog(connection, fileName, ScheduleTxtLog.Success.GetEnumDescription(), WriteCount);
                }
                catch (Exception ex)
                {
                    UpdataLog(connection, fileName, ScheduleTxtLog.Error.GetEnumDescription(), 0);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 新增一筆Log資料
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fileName">檔案名稱</param>
        public void InsertLog(SqlConnection connection, string fileName)
        {
            string insertSql = "INSERT INTO ScheduleTxtLog (StartTime, Schedulestate,FilesName) VALUES (@StartTime, @Schedulestate,@fileName)";
            SqlCommand command = new SqlCommand(insertSql, connection);
            command.Parameters.AddWithValue("@StartTime", DateTime.Now);
            command.Parameters.AddWithValue("@Schedulestate", ScheduleTxtLog.Running.GetEnumDescription());
            command.Parameters.AddWithValue("@fileName", fileName);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// 這邊分為失敗更新，與成功更新(查詢條件為"檔案名稱、失敗時間")
        /// 失敗更新 : 筆數會為0，狀態會失敗，失敗時間會上去
        /// 成功更新 : 筆數會為當下筆數，狀態會成功，完成時間會上去
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fileName"></param>
        /// <param name="Schedulestate"></param>
        /// <param name="Count"></param>
        public void UpdataLog(SqlConnection connection, string fileName, string Schedulestate, int Count)
        {
            string UpdateSql = "update ScheduleTxtLog set Schedulestate = @Schedulestate,EndTime = @EndTime ,OutPutCount = @OutPutCount where FilesName = @fileName and EndTime is null";
            SqlCommand command = new SqlCommand(UpdateSql, connection);
            command.Parameters.AddWithValue("@Schedulestate", Schedulestate);
            command.Parameters.AddWithValue("@EndTime", DateTime.Now);
            command.Parameters.AddWithValue("@fileName", fileName);
            command.Parameters.AddWithValue("@OutPutCount", Count);
            command.ExecuteNonQuery();
        }
    }
}
