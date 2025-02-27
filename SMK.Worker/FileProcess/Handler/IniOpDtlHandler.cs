using System;
using System.Collections.Generic;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class IniOpDtlHandler : FileInHandler<IniOpDtl>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"B7_(\w+)_OPDTL\.txt";

        public override IniOpDtl Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into iniOpDtl(data_id,fee_ym,ExamYear,InstructExamYear,appl_type,HospID,appl_date,case_type,seq_no,"
            // sql += "cure_item1, cure_item2, cure_item3, cure_item4, func_type, func_date, cure_e_date,"
            // sql += "birthday,id,func_seq_no,pay_type,part_code,icd9cm_code,icd9cm_code1,icd9cm_code2,drug_days,rel_mode,prsn_id,drug_prsn_id,"
            // sql += "drug_dot,cure_dot,diag_code,diag_dot,dsvc_code,dsvc_dot,exp_dot,part_amt,appl_dot,Id_Sex,area_service,supp_area,real_hosp_id,"
            // sql += "hosp_data_type,agency_part_amt,name,appl_cause_mark,icd10cm_code3,icd10cm_code4,met_dot,corr_hosp_id,tran_date) "
            // sql += " values("
            // sql += "'" & wkData_id & "',"
            // sql += "'" & strLineArray(0).ToString.Trim() & "',"
            // sql += "NULL,"
            // sql += "NULL,"
            // sql += "'" & strLineArray(1).ToString.Trim() & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "" & CheckZero(strLineArray(5).ToString.Trim()) & ","
            // sql += "'" & strLineArray(6).ToString.Trim() & "',"
            // sql += "'" & strLineArray(7).ToString.Trim() & "',"
            // sql += "'" & strLineArray(8).ToString.Trim() & "',"
            // sql += "'" & strLineArray(9).ToString.Trim() & "',"
            // sql += "'" & strLineArray(10).ToString.Trim() & "',"
            // sql += "'" & strLineArray(11).ToString.Trim() & "',"
            // sql += "NULL,"
            // sql += "'" & strLineArray(13).ToString.Trim() & "',"
            // sql += "'" & strLineArray(14).ToString.Trim() & "',"
            // sql += "'" & strLineArray(15).ToString.Trim() & "',"
            // sql += "'" & strLineArray(16).ToString.Trim() & "',"
            // sql += "'" & strLineArray(17).ToString.Trim() & "',"
            // sql += "'" & Mid(strLineArray(18).ToString.Trim(), 1, 5) & "',"
            // sql += "'" & Mid(strLineArray(19).ToString.Trim(), 1, 5) & "',"
            // sql += "'" & Mid(strLineArray(20).ToString.Trim(), 1, 5) & "',"
            // sql += "" & CheckZero(strLineArray(21).ToString.Trim()) & ","
            // sql += "'" & strLineArray(22).ToString.Trim() & "',"
            // sql += "'" & strLineArray(23).ToString.Trim() & "',"
            // sql += "'" & strLineArray(24).ToString.Trim() & "',"
            // sql += "" & strLineArray(25).ToString.Trim() & ","
            // sql += "" & strLineArray(26).ToString.Trim() & ","
            // sql += "'" & strLineArray(27).ToString.Trim() & "',"
            // sql += "" & strLineArray(28).ToString.Trim() & ","
            // sql += "'" & strLineArray(29).ToString.Trim() & "',"
            // sql += "" & strLineArray(30).ToString.Trim() & ","
            // sql += "" & strLineArray(31).ToString.Trim() & ","
            // sql += "" & strLineArray(32).ToString.Trim() & ","
            // sql += "" & strLineArray(33).ToString.Trim() & ","
            // sql += "'" & GetMFString(strLineArray(14).ToString.Trim()) & "',"
            // sql += "'" & strLineArray(37).ToString.Trim() & "',"
            // sql += "'" & strLineArray(38).ToString.Trim() & "',"
            // sql += "'" & strLineArray(39).ToString.Trim() & "',"
            // sql += "'" & strLineArray(36).ToString.Trim() & "',"
            // sql += "" & strLineArray(40).ToString.Trim() & ","
            // sql += "N'" & Replace(strLineArray(41).ToString.Trim(), ",", "") & "',"
            // sql += "'" & strLineArray(42).ToString.Trim() & "',"
            // sql += "'" & strLineArray(43).ToString.Trim() & "',"
            // sql += "'" & strLineArray(44).ToString.Trim() & "',"
            // sql += "" & strLineArray(45).ToString.Trim() & ","
            // sql += "'" & strLineArray(46).ToString.Trim() & "',"
            // sql += "'" & strLineArray(47).ToString.Trim() & "')"

            return new IniOpDtl()
            {
                DataId = values[35].Trim(),
                FeeYm = values[0].Trim(),
                ExamYear = null,
                InstructExamYear = null,
                ApplType = values[1].Trim(),
                HospId = values[2].Trim(),
                ApplDate = values[3].Trim(),
                CaseType = values[4].Trim(),
                SeqNo = values[5].Trim().ToInt32(),
                CureItem1 = values[6].Trim(),
                CureItem2 = values[7].Trim(),
                CureItem3 = values[8].Trim(),
                CureItem4 = values[9].Trim(),
                FuncType = values[10].Trim(),
                FuncDate = values[11].Trim(),
                CureEDate = values[12].Trim(),
                Birthday = values[13].Trim(),
                Id = values[14].Trim(),
                FuncSeqNo = values[15].Trim(),
                PayType = values[16].Trim(),
                PartCode = values[17].Trim(),
                Icd9cmCode = values[18].Trim(),
                Icd9cmCode1 = values[19].Trim(),
                Icd9cmCode2 = values[20].Trim(),
                DrugDays = values[21].Trim().ToInt32(),
                RelMode = values[22].Trim(),
                PrsnId = values[23].Trim(),
                DrugPrsnId = values[24].Trim(),
                DrugDot = values[25].Trim().ToInt32(),
                CureDot = values[26].Trim().ToInt32(),
                DiagCode = values[27].Trim(),
                DiagDot = values[28].Trim().ToInt32(),
                DsvcCode = values[29].Trim(),
                DsvcDot = values[30].Trim().ToInt32(),
                ExpDot = values[31].Trim().ToInt32(),
                PartAmt = values[32].Trim().ToInt32(),
                ApplDot = values[33].Trim().ToInt32(),
                IdSex = values[14].Trim().ToGender(),
                AreaService = values[37].Trim(),
                SuppArea = values[38].Trim(),
                RealHospId = values[39].Trim(),
                HospDataType = values[36].Trim(),
                AgencyPartAmt = values[40].Trim().ToDecimal(),
                Name = values[41].Trim(),
                ApplCauseMark = values[42].Trim(),
                Icd10cmCode3 = values[43].Trim(),
                Icd10cmCode4 = values[44].Trim(),
                MetDot = values[45].Trim().ToInt32(),
                CorrHospId = values[46].Trim(),
                TranDate = values[47].Trim(),
            };
        }
    }
}