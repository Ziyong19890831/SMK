using System;
using System.Collections.Generic;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class IniOpOrdHandler: FileInHandler<IniOpOrd>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"B7_(\w+)_OPORD\.txt";

        public override IniOpOrd Transform(string[] values, Dictionary<string, object> args)
        {
            // sql = "insert into iniOpOrd(data_id,order_seq_no,fee_ym,order_type,order_code,rel_mode,drug_num,"
            // sql += "drug_fre,drug_path,order_uprice,order_qty,order_dot,exe_prsn_id,cure_path,order_drug_day,tran_date) "
            // sql += " values("
            // sql += "'" & wkData_id & "',"
            // sql += "" & CInt(strLineArray(1).ToString.Trim()) & ","
            // sql += "'" & strLineArray(12).ToString.Trim() & "',"
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & strLineArray(3).ToString.Trim() & "',"
            // sql += "'" & strLineArray(4).ToString.Trim() & "',"
            // sql += "'" & Mid(StrConv(strLineArray(5).ToString.Trim(), VbStrConv.Narrow), 1, 6) & "',"
            // sql += "'" & Mid(StrConv(strLineArray(6).ToString.Trim(), VbStrConv.Narrow), 1, 18).Replace("'", "") & "',"
            // sql += "'" & Mid(strLineArray(7).ToString.Trim(), 1, 15) & "',"
            // sql += "" & strLineArray(8).ToString.Trim() & ","
            // sql += "" & strLineArray(9).ToString.Trim() & ","
            // sql += "" & strLineArray(10).ToString.Trim() & ","
            // sql += "'" & strLineArray(11).ToString.Trim() & "',"
            // sql += "'" & strLineArray(13).ToString.Trim() & "',"
            // sql += "" & strLineArray(14).ToString.Trim() & ","
            // sql += "'" & strLineArray(15).ToString.Trim() & "')"
            return new IniOpOrd()
            {
                DataId = values[0].Trim(),
                OrderSeqNo = Convert.ToInt32(values[1].Trim()),
                FeeYm = values[12].Trim(),
                OrderType = values[2].Trim(),
                OrderCode = values[3].Trim(),
                RelMode = values[4].Trim(),
                DrugNum = values[5].Trim().SafeSubstring(0, 6),
                DrugFre = values[6].Trim().SafeSubstring(0, 18).Trim('\''),
                DrugPath = values[7].Trim().SafeSubstring(0, 15),
                OrderUprice = Convert.ToDecimal(values[8].Trim()),
                OrderQty = Convert.ToDecimal(values[9].Trim()),
                OrderDot = Convert.ToInt32(values[10].Trim()),
                ExePrsnId = values[11].Trim(),
                CurePath = values[13].Trim(),
                OrderDrugDay = Convert.ToInt32(values[14].Trim()),
                TranDate = values[15].Trim(),
            };
        }
    }
}
