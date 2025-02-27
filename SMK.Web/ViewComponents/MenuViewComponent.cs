using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SMK.Web.ViewComponents
{
    /// <summary>
    /// 用戶資訊
    /// </summary>
    public class MenuViewComponent : ViewComponent
    {

        public MenuViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
