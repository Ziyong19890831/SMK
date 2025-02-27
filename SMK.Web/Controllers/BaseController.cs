using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SMK.Data.Dto;
using SMK.Web.AppScope;
using SMK.Web.Models;

namespace SMK.Web.Controllers
{
    public class BaseController : Controller
    {
        public string GetControllerName<T>() where T : Controller
        {
            return typeof(T).Name.Replace("Controller", "");
        }

        protected IActionResult View<T>(string viewName, LogicRtnModel<T> rtnModel) {

            ModelState.Clear();

            bindingViewContext(rtnModel);

            return View(viewName,rtnModel.Data);
        }

        /// <summary>
        /// TranToPass Data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rtnModel"></param>
        /// <returns></returns>
        protected IActionResult View<T>(LogicRtnModel<T> rtnModel)
        {
            ModelState.Clear();

            bindingViewContext(rtnModel);

            return View(rtnModel.Data);
        }


        protected IActionResult RedirectTo<T>(
            LogicRtnModel<T> rtnModel,
            string action,
            string controller = "",
            object routeValues = null
            )
        {
            ModelState.Clear();

            bindingViewContext(rtnModel);

            if (string.IsNullOrEmpty(controller))
            {
                return RedirectToAction(action);
            }
            else
            {
                return RedirectToAction(action, controller, routeValues);
            }
        }


        private void bindingViewContext<T>(LogicRtnModel<T> rtnModel)
        {
            // auto binding
            ViewBag.ExtendData = rtnModel.ExtendData;
            ViewBag.Stacktrace = rtnModel.StackTrace;

            //
            var msg = new MsgComponentModel();

            if (!string.IsNullOrEmpty(rtnModel.StackTrace))
            {
                msg.Type = Models.AlertMsgType.Error;
                msg.Title = rtnModel.ErrMsg;
            }
            else
            {
                msg.Type = rtnModel.IsSuccess ? Models.AlertMsgType.Success : Models.AlertMsgType.Warning;
                switch (msg.Type)
                {
                    case Models.AlertMsgType.Success:
                        msg.Title = rtnModel.Msg;
                        break;
                    case Models.AlertMsgType.Warning:
                        msg.Title = rtnModel.ErrMsg.Replace("\r\n","");
                        break;

                }
            }

            if (!TempData.Keys.Contains("$MsgModel"))
            {
                TempData.Put("$MsgModel", msg);
            }
        }

    }
}