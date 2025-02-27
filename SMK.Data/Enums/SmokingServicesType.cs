using System.ComponentModel;

namespace SMK.Data.Enums
{
    public class SmokingServicesType
    {
        public enum SmokingServicesTypeEnums
        {
            [Description("治療")]
            Treat = 1,
            [Description("衛教")]
            HealthEducation = 2,
        }
    }
}
