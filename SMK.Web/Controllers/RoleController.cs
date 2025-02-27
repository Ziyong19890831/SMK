using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SMK.Data.Dto;
using SMK.Data.Entity;
using SMK.Data.Utility;
using SMK.Web.AppScope.Filters;
using SMK.Web.Controllers;
using SMK.Web.Services;
using SMK.Web.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yozian.Extension;

namespace SMK.Web.Controllers
{
    [EmpAuthorized]
    [PrivilegeGuard]
    [Route("[controller]/[action]")]
    public class RoleController : BaseController
    {
        private readonly RoleService roleService;
        private readonly PrivilegeService privilegeService;

        public RoleController(
            RoleService roleService,
            PrivilegeService privilegeService
            )
        {
            this.roleService = roleService;
            this.privilegeService = privilegeService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var rtnModel = await roleService.Query(context => context.Role);

            return View(rtnModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            model.Id = MyGuid.NewGuid();
            var rtnModel = await roleService
                .Create(model, new RoleValidator().Validate);
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(RoleController.List));
            }
            else
            {
                return View(rtnModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var rtnModel = await roleService
                .FindOne<Role>((roles) => roles.Where(x => x.Id.Equals(id)));

            return View(rtnModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Role model)
        {
            var rtnModel = await roleService
                .Update(
                    model,
                    new RoleValidator().Validate,
                    true,
                    x => x.Name
                );
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel, nameof(this.List));
            }
            else
            {
                return View(rtnModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string id)
        {
            var rtnModel = await roleService.Remove(id);
            return Json(rtnModel);// return RedirectToAction(nameof(RoleController.List));
        }

        #region  Details 成員
        [HttpGet]
        [Route("{roleId}")]
        public async Task<IActionResult> MemberList(string roleId, RoleQueryModel model)
        {
            if (string.IsNullOrEmpty(roleId))
                return RedirectToAction(nameof(RoleController.List));
            model.Id = roleId;
            var rtnModel = await roleService.QueryEmpRoles(model);

            return View(rtnModel);
        }

        [HttpGet]
        [Route("{roleId}")]
        public async Task<IActionResult> JoinRole(string roleId)
        {
            var rtnModel = await roleService.FindOne<Role>(s => s.Where(x => x.Id.Equals(roleId)));
            ViewBag.Role = rtnModel.Data;
            ViewBag.EmpData = new GenEmpData();
            return View(rtnModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{roleId}")]
        public async Task<IActionResult> JoinRole(string roleId, GenEmpData model)
        {
            var rtnModel = await roleService.EmpJoinRole(roleId, model);
            ViewBag.Role = rtnModel.ExtendData;
            ViewBag.EmpData = rtnModel.Data;
            if (rtnModel.IsSuccess)
            {
                return RedirectTo(rtnModel,
                    nameof(RoleController.MemberList),
                    GetControllerName<RoleController>(),
                   new
                   {
                       roleId = roleId
                   });
            }
            else
            {
                return View(rtnModel);
            }
        }


        [HttpPost]
        [Route("{roleId}/{empId}")]
        public async Task<IActionResult> LeaveRole(string roleId, string empId)
        {
            var rtnModel = await roleService.EmpLeaveRole(roleId, empId);
            return Json(rtnModel);
        }


        #endregion


        #region 權限

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Privilege(string id)
        {
            var rtnModel = await roleService.GetAllPrivilegesByRole(id);
            return View(rtnModel);
        }

        [HttpPost]
        public async Task<IActionResult> SwitchRolePrivilege(
            string roleId,
            string privilegeId,
            bool enable)
        {
            var rtnModel = await roleService.SwitchRolePrivilege(roleId, privilegeId, enable);
            return Json(rtnModel);
        }

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }


    }
}