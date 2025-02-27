using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SMK.Web.AppScope.Filters;
using SMK.Web.Models;
using SMK.Web.Services;
using SMK.Web.Services.Foundation;

namespace SMK.Web.Controllers
{
    public class HomeController : BaseController
    {
        AuthService authService;
        SessionManager smgr;
        private readonly CallBoardService callBoardService;
        private readonly string _folder;

        public HomeController(
            AuthService authService,
            SessionManager sessionManager,
            CallBoardService callBoardServices,
            IWebHostEnvironment env
        )
        {
            this.authService = authService;
            this.smgr = sessionManager;
            callBoardService = callBoardServices;
            _folder = $@"{env.WebRootPath}\CallBoardFolder";
        }
        [EmpAuthorized]
        public IActionResult Index()
        {
            var rtnModel = callBoardService.QueryCallBoard();
            if (rtnModel.IsSuccess)
            {
                rtnModel.Data = rtnModel.Data.Where(x => x.Condition == true && x.Action == false).ToList();
                List<CallBoardViewModel> bModel = new List<CallBoardViewModel>();
                string json = string.Empty;
                foreach (var item in rtnModel.Data)
                {
                    json = JsonConvert.SerializeObject(item);
                    CallBoardViewModel callBoardViewModel = JsonConvert.DeserializeObject<CallBoardViewModel>(json);
                    callBoardViewModel.Files = callBoardService.GetFilesData(_folder, item.SysNo);
                    bModel.Add(callBoardViewModel);
                }
                rtnModel.Data = bModel;
                return View(rtnModel);
            }
            return View();
        }

        public IActionResult Login([FromQuery] string redirect)
        {
            ViewBag.Redirect = redirect;
#if DEBUG
            ViewBag.Account = "Admin";
            ViewBag.Pwd = "1qaz@WSX";
            ViewBag.CAPTCHA = "1234";
            ViewBag.AutoLogin = true;
#endif

            return View();
        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(
            string account,
            string pwd,
            string redirect
        )
        {

            string LoginMsg = await authService.Login(account, pwd);

            if (!string.IsNullOrEmpty(LoginMsg))
            {
                ViewBag.Msg = LoginMsg;
                ViewBag.ForceChangePwd = false;
                return View();
            }
            else
            {
#if !DEBUG
                if (await authService.ForceChangePwd(account))
                {
                    return RedirectToAction(nameof(HomeController.ForceChangePwd), GetControllerName<HomeController>());
                }
#endif
            }
            if (!string.IsNullOrEmpty(redirect))
            {
                return Redirect(redirect);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), GetControllerName<HomeController>());
            }

        }
        [HttpGet]
        public IActionResult CheckCode()
        {
            return CreateCheckCodeImage(GenerateCheckCode());
        }
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            Random random = new Random();

            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                code = (char)('0' + (char)(number % 10));
                checkCode += code.ToString();
            }
#if DEBUG
            return "1234";
#else
            return checkCode;
#endif

        }

        private IActionResult CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return StatusCode(500);

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成隨機生成器
                Random random = new Random();

                //清空圖片背景色
                g.Clear(Color.White);

                //畫圖片的背景噪音線
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //畫圖片的前景噪音點
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //畫圖片的邊框線
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.Cookies.Append("CheckCode", checkCode);
                return File(ms.ToArray(), "image/Gif");
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public IActionResult ForceChangePwd()
        {
            return View();
        }

        public IActionResult UnAuthrized()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <returns></returns>
        [EmpAuthorized]
        [HttpGet]
        public IActionResult ChangeMyPwd()
        {
            return View();
        }
        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EmpAuthorized]
        public async Task<IActionResult> ChangeMyPwd(string pwd)
        {
            var rtnModel = await this.authService.UpdateMyPwd(pwd);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(HomeController.Index), GetControllerName<HomeController>());
            }
            return View(rtnModel);
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EmpAuthorized]
        public IActionResult LogOff()
        {
            this.smgr.Remove(authService.Identity);
            return RedirectToAction(nameof(HomeController.Index), GetControllerName<HomeController>());
        }
    }
}