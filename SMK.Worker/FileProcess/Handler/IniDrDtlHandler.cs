using System;
using System.Collections.Generic;
using System.Linq;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.Extensions;
using Yozian.Extension;

namespace SMK.Worker.FileProcess.Handler
{
    public class IniDrDtlHandler : FileInHandler<IniDrDtl>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"B7_(\w+)_DRDTL\.txt";

        public override FileColumn[] Columns
        {
            get
            {
                return new FileColumn[]
                {
                  new FileColumn<string>("DataId", 0) { Transform = x => x.Trim() },
                  new FileColumn<string>("", 0) { Transform = x => x.Trim() },
                  new FileColumn<string>("FeeYm", 0) { Transform = x => x.Trim() },
                  new FileColumn<string>("ExamYear", 0) { Transform = x => x.Trim() },
                };
            }
        }
        public override IniDrDtl Transform(string[] values, Dictionary<string, object> args)
        {
            // wkData_id = strLineArray(3).ToString.Trim() + strLineArray(0).ToString.Trim() + _
            // CStr(Mid(strLineArray(4).ToString.Trim(), 1, 4) - 1911).PadLeft(3, "0") + Mid(strLineArray(4).ToString.Trim(), 5, 2) + Mid(strLineArray(4).ToString.Trim(), 7, 2) + _
            // strLineArray(5).ToString.Trim().PadLeft(2, "0") + strLineArray(6).ToString.Trim().PadLeft(6, "0") + "30"
            var dataId = values[3].Trim() +
                         values[0].Trim() +
                         values[4].Trim('/').ToTaiwanDateFromYYYYMMDD() +
                         values[5].Trim().PadLeft(2, '0') +
                         values[6].Trim().PadLeft(6, '0') +
                         "30";
            // sql = "insert into iniDrDtl(data_id,HospID,fee_ym,ExamYear,InstructExamYear,appl_type,appl_date,case_type,seq_no,"
            // sql += "cure_item1,cure_item2,cure_item3,cure_item4,func_type,func_date,"
            // sql += "rel_date,birthday,id,func_seq_no,pay_type,part_code,icd9cm_code,icd9cm_code1,icd9cm_code2,drug_days,"
            // sql += "prsn_id,drug_prsn_id,drug_dot,cure_dot,dsvc_code,dsvc_dot,exp_dot,part_amt,appl_dot,orig_hosp_id,Id_Sex,"
            // sql += "orig_case_type,other_part_amt,appl_cause_mark,icd10cm_code2,icd10cm_code3,icd10cm_code4,corr_hosp_id,area_service,name,tran_date)"
            // sql += " values("
            // sql += "'" & wkData_id & "',"
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & Mid(strLineArray(2).ToString.Trim(), 1, 4) & "',"
            // sql += "NULL,"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "'" & strLineArray(5).ToString.Trim() & "',"
            // sql += "" & strLineArray(6).ToString.Trim() & ","
            // sql += "'" & strLineArray(10).ToString.Trim() & "',"
            // sql += "'" & strLineArray(11).ToString.Trim() & "',"
            // sql += "'" & strLineArray(12).ToString.Trim() & "',"
            // sql += "'" & strLineArray(13).ToString.Trim() & "',"
            // sql += "'" & strLineArray(14).ToString.Trim() & "',"
            // sql += "'" & strLineArray(15).ToString.Trim() & "',"
            // sql += "'" & strLineArray(16).ToString.Trim() & "',"
            // sql += "'" & strLineArray(17).ToString.Trim() & "',"
            // sql += "'" & strLineArray(18).ToString.Trim() & "',"
            // sql += "'" & strLineArray(29).ToString.Trim() & "',"
            // sql += "'" & strLineArray(19).ToString.Trim() & "',"
            // sql += "'" & strLineArray(23).ToString.Trim() & "',"
            // sql += "'" & Mid(strLineArray(20).ToString.Trim(), 1, 5) & "',"
            // sql += "'" & Mid(strLineArray(21).ToString.Trim(), 1, 5) & "',"
            // sql += "'" & Mid(strLineArray(22).ToString.Trim(), 1, 5) & "',"
            // sql += "" & strLineArray(25).ToString.Trim() & ","
            // sql += "'" & strLineArray(26).ToString.Trim() & "',"
            // sql += "'" & strLineArray(27).ToString.Trim() & "',"
            // sql += "" & strLineArray(28).ToString.Trim() & ","
            // sql += "" & strLineArray(30).ToString.Trim() & ","
            // sql += "'" & strLineArray(31).ToString.Trim() & "',"
            // sql += "" & strLineArray(32).ToString.Trim() & ","
            // sql += "" & strLineArray(35).ToString.Trim() & ","
            // sql += "" & strLineArray(34).ToString.Trim() & ","
            // sql += "" & strLineArray(33).ToString.Trim() & ","
            // sql += "'" & strLineArray(7).ToString.Trim() & "',"
            // sql += "'" & GetMFString(strLineArray(18).ToString.Trim()) & "',"
            // sql += "'" & strLineArray(9).ToString.Trim() & "',"
            // sql += "" & strLineArray(38).ToString.Trim() & ","
            // sql += "'" & strLineArray(39).ToString.Trim() & "',"
            // sql += "'" & strLineArray(40).ToString.Trim() & "',"
            // sql += "'" & strLineArray(41).ToString.Trim() & "',"
            // sql += "'" & strLineArray(42).ToString.Trim() & "',"
            // sql += "'" & strLineArray(43).ToString.Trim() & "',"
            // sql += "'" & strLineArray(44).ToString.Trim() & "',"
            // sql += "'" & strLineArray(36).ToString.Trim() & "',"
            // sql += "'" & strLineArray(45).ToString.Trim() & "')"

            return new IniDrDtl()
            {
                DataId = dataId,
                HospId = values[0].Trim(),
                FeeYm = values[2].Trim(),
                ExamYear = values[2].Trim().Substring(0, 4),
                InstructExamYear = null,
                ApplType = values[3].Trim(),
                ApplDate = values[4].Trim(),
                CaseType = values[5].Trim(),
                SeqNo = Convert.ToInt32(values[6].Trim()),
                CureItem1 = values[10].Trim(),
                CureItem2 = values[11].Trim(),
                CureItem3 = values[12].Trim(),
                CureItem4 = values[13].Trim(),
                FuncType = values[14].Trim(),
                FuncDate = values[15].Trim(),
                RelDate = values[16].Trim(),
                Birthday = values[17].Trim(),
                Id = values[18].Trim(),
                FuncSeqNo = values[29].Trim(),
                PayType = values[19].Trim(),
                PartCode = values[23].Trim(),
                Icd9cmCode = values[20].Trim(),
                Icd9cmCode1 = values[21].Trim(),
                Icd9cmCode2 = values[22].Trim(),
                DrugDays = Convert.ToInt32(values[25].Trim()),
                PrsnId = values[26].Trim(),
                DrugPrsnId = values[27].Trim(),
                DrugDot = Convert.ToInt32(values[28].Trim()),
                CureDot = Convert.ToInt32(values[30].Trim()),
                DsvcCode = values[31].Trim(),
                DsvcDot = Convert.ToInt32(values[32].Trim()),
                ExpDot = Convert.ToInt32(values[35].Trim()),
                PartAmt = Convert.ToInt32(values[34].Trim()),
                ApplDot = Convert.ToInt32(values[33].Trim()),
                OrigHospId = values[7].Trim(),
                IdSex = values[18].Trim().ToGender(),
                OrigCaseType = values[9].Trim(),
                OtherPartAmt = Convert.ToInt32(values[38].Trim()),
                ApplCauseMark = values[39].Trim(),
                Icd10cmCode2 = values[40].Trim(),
                Icd10cmCode3 = values[41].Trim(),
                Icd10cmCode4 = values[42].Trim(),
                CorrHospId = values[43].Trim(),
                AreaService = values[44].Trim(),
                Name = values[36].Trim(),
                TranDate = values[45].Trim(),
            };
        }

        public override int Length { get; } = 0;
    }
}