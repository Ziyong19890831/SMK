using System;
using System.Collections.Generic;
using SMK.Data;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class IniDrOrdHandler: FileInHandler<IniDrOrd>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"B7_(\w+)_DRORD\.txt";

        public override IniDrOrd Transform(string[] values, Dictionary<string, object> args)
        {
            // wkData_id = strLineArray(3).ToString.Trim() + strLineArray(0).ToString.Trim() + _
            // CStr(Mid(strLineArray(4).ToString.Trim(), 1, 4) - 1911).PadLeft(3, "0") + Mid(strLineArray(4).ToString.Trim(), 5, 2) + Mid(strLineArray(4).ToString.Trim(), 7, 2) + _
            // strLineArray(5).ToString.Trim().PadLeft(2, "0") + strLineArray(6).ToString.Trim().PadLeft(6, "0") + "30"
            // sql = "insert into iniDrOrd(data_id,order_seq_no,fee_ym,order_type,order_code,drug_num,drug_fre,"
            // sql += "drug_path,order_uprice,order_qty,order_dot,order_drug_day,exe_prsn_id,tran_date) "
            // sql += " values("
            // sql += "'" & wkData_id & "',"
            // sql += "" & strLineArray(7).ToString.Trim() & ","
            // sql += "'" & strLineArray(2).ToString.Trim() & "',"
            // sql += "'" & Mid(strLineArray(8).ToString.Trim(), 1, 1) & "',"
            // sql += "'" & Mid(strLineArray(9).ToString.Trim(), 1, 12) & "',"
            // sql += "" & Mid(StrConv(strLineArray(10).ToString.Trim(), VbStrConv.Narrow), 1, 6) & ","
            // sql += "'" & Mid(StrConv(strLineArray(11).ToString.Trim(), VbStrConv.Narrow), 1, 18).Replace("'", "") & "',"
            // sql += "'" & Mid(strLineArray(12).ToString.Trim(), 1, 15) & "',"
            // sql += "" & strLineArray(14).ToString.Trim() & ","
            // sql += "" & strLineArray(13).ToString.Trim() & ","
            // sql += "" & strLineArray(15).ToString.Trim() & ","
            // sql += "" & strLineArray(16).ToString.Trim() & ","
            // sql += "'" & strLineArray(17).ToString.Trim() & "',"
            // sql += "'" & strLineArray(18).ToString.Trim() & "')"

            var dataId = values[3].Trim() +
                         values[0].Trim() +
                         values[4].Trim('/').ToTaiwanDateFromYYYYMMDD() +
                         values[5].Trim().PadLeft(2, '0') +
                         values[6].Trim().PadLeft(6, '0') +
                         "30";
            return new IniDrOrd()
            {
                DataId = dataId,
                OrderSeqNo = Convert.ToInt32(values[7].Trim()),
                FeeYm = values[2].Trim(),
                OrderType = values[8].Trim(),
                OrderCode = values[9].Trim(),
                DrugNum = values[10].Trim(),
                DrugFre = values[11].Trim().Trim('\''),
                DrugPath = values[12].Trim(),
                OrderUprice = Convert.ToDecimal(values[14].Trim()),
                OrderQty = Convert.ToDecimal(values[13].Trim()),
                OrderDot = Convert.ToInt32(values[15].Trim()),
                OrderDrugDay = Convert.ToInt32(values[16].Trim()),
                ExePrsnId = values[17].Trim(),
                TranDate = values[18].Trim(),
            };
        }
    }
}
