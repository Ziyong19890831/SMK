using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SMK.Web.ViewComponents
{
    /// <summary>
    /// 用戶資訊
    /// </summary>
    public class MenuUserViewComponent : ViewComponent
    {

        public MenuUserViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
