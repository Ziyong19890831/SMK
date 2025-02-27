using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SMK.Data.Entity;
using SMK.Web.Extensions;

namespace SMK.Worker.FileProcess.Handler
{
    public class CstQsData2Handler : FileInHandler<MhbtQsData2>
    {
        public override int Header { get; set; } = 0;
        public override string FilenamePattern => @"CST_QS_DATA2.txt";
        
        public override string[] Parse(string line)
        {
            return line.Split(",");
        }

        public override MhbtQsData2 Transform(string[] values, Dictionary<string, object> args)
        {
            DateTime now = (DateTime) args["Now"];
            return new MhbtQsData2()
            {
                HospId = values[0].Trim(),
                ID = values[1].Trim(),
                Birthday = values[2].Trim(),
                FuncDate = values[3].Trim(),
                CureStage = values[4].Trim(),
                ExamYear = values[5].Trim(),
                Cure_Type = values[6].Trim(),
                HospSeqNo = values[7].Trim(),
                BaseWeight = decimal.TryParse(values[8].Trim(), out decimal baseWeight) ? baseWeight : 0,
                Height = decimal.TryParse(values[9].Trim(), out decimal height) ? height : 0,
                PrsnID = values[10].Trim(),
                Trace_Co_Check3 = values[11].Trim(),
                Trace_Date3 = values[12].Trim(),
                Cure_State3 = values[13].Trim(),
                Trace_State3 = values[14].Trim(),
            };
        }
    }
}
