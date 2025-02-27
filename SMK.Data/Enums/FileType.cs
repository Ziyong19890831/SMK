using System.ComponentModel;
using SMK.Data.Attributes;

namespace SMK.Data.Enums
{
    /// <summary>
    /// 上傳檔案來源類型
    /// </summary>
    public enum FileType
    {
        //VPN=0,
        //健保檔=1,
        //健保過卡=2
        [Description("藥局治療明細資料")]
        [FilenamePattern("_DRDTL")]
        iniDrDtlTxt = 0,
        [Description("藥局醫令明細資料")]
        [FilenamePattern("_DRORD")]
        iniDrOrdTxt = 1,
        [Description("門診治療明細資料")]
        [FilenamePattern("_OPDTL")]
        iniOpDtlTxt = 2,
        [Description("門診醫令明細資料")]
        [FilenamePattern("_OPORD")]
        iniOpOrdTxt = 3,
        [Description("個案基本資料")]
        AgentPatientTxt = 4,
        [Description("門診戒菸治療狀況")]
        QsCureTxt = 5,
        [Description("門診戒菸資料")]
        QsDataTxt = 6,
        [Description("門診戒菸副作用或戒斷症狀")]
        QsStateTxt = 7,
        [Description("健保卡過卡資料")]
        ICCardTxt = 8,
        [Description("專審預抽個案資料")]
        SamplingListTxt = 9,
        [Description("醫事機構資料")]
        HospBscAllTxt = 10,
        [Description("戒菸率調查檔")]
        QuitDataAllTxt = 11,
        [Description("門診戒菸資料2")]
        QsData2Txt = 12,
    }
}
