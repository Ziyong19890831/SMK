using SMK.Data.Entity;

namespace SMK.Web.Models
{
    public class PrsnEmailViewModel : PrsnEmail
    {
        public PrsnEmailViewModel(PrsnEmail prsnEmail)
        {
            PrsnId = prsnEmail.PrsnId;
            Pemail = prsnEmail.Pemail;
        }
    }
}