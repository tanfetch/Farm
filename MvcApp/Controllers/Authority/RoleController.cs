using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Farm.Authority.Users;
using Farm.Authority.Filter;
using Farm.Authority.Permission;
using System.Reflection;
using Farm.Authority.DataContext;
using System.ComponentModel;
using Farm.AppCommon;
using Farm.AppCommon.Extensions;

namespace Farm.Controllers.Authority
{

    public class RoleController : BaseController
    {

        [HttpGet]
        [Description("查看权限组列表")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看权限组列表")]
        public ActionResult List(FormCollection fc)
        {
            AuthorityRepository rps = new AuthorityRepository();
            return base.DataGrid<tbRole, IBaseTable>(rps);
        }

        [HttpGet]
        [Description("增加权限组,修改权限组信息")]
        public ActionResult Update(string go)
        {
            if (go == "Edit")
            {
                int id;
                Int32.TryParse(Request["id"], out id);
                return base.Entity<tbRole, IBaseTable>(new AuthorityRepository(), id, "Edit");
            }

            return PartialView("Create");
        }

        [HttpPost]
        [Description("增加权限组,修改权限组信息")]
        public ActionResult Update()
        {
            return base.UpdateEntity<tbRole>();
        }

        [Description("删除权限组")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbRole>(id);
        }

        [Description("禁用或启用权限组")]
        public ActionResult ToggleInvalid(int id, bool value)
        {
            return base.UpdateProperty<tbRole>(id, "disabled", value);
        }

        [HttpGet]
        [Description("修改权限组的权限项")]
        public ActionResult UpdateRoles(int id)
        {
            var actions = GetAllActionByAssembly();
            actions = actions.Distinct(new FastPropertyComparer<ActionPermission>("actionMark"));
            var rsp = new AuthorityRepository();
            foreach(var item in actions)
            {
                item.roleID = id;

                var m =rsp.GetEntitie<tbPermission>(
                    p=>p.roleID==id && p.actionName==item.actionName && p.controllerName==item.controllerName);
                item.isValid = (m!=null);
            }

            return base.DataGrid<ActionPermission>(actions.AsQueryable());
        }

        [HttpPost]
        [Description("修改权限组的权限项")]
        public ActionResult UpdateRoles(int id, tbPermission[] roles)
        {
            string result = "";
            var rsp = new AuthorityRepository();
            result = rsp.Delete<tbPermission>(p => p.roleID == id);
            if (!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result, false),JsonRequestBehavior.AllowGet);

            if (roles != null)
            {
                foreach (var item in roles)
                {
                    item.roleID = id;
                    result = rsp.Inset<tbPermission>(item);
                    if (!string.IsNullOrEmpty(result))
                        return Json(JSHelper.JsonMessage(result, false), JsonRequestBehavior.AllowGet);
                }
            }

            return Json(JSHelper.JsonMessage("操作成功",true),JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private IEnumerable<ActionPermission> GetAllActionByAssembly()
        {
            var result = new List<ActionPermission>();

            var types = Assembly.Load("MvcApp").GetTypes();

            foreach (var type in types)
            {
                if (type.BaseType == null)
                    continue;

                if (!type.IsGenericType && type.BaseType.Name.Contains("BaseController"))//如果是Controller
                {
                    var members = type.GetMethods();
                    foreach (var member in members)
                    {
                        if (member.ReturnType.Name == "ActionResult")//如果是Action
                        {

                            var ap = new ActionPermission();

                            ap.actionName = member.Name;
                            ap.controllerName = member.DeclaringType.Name.Substring(0, member.DeclaringType.Name.Length - 10); // 去掉“Controller”后缀

                            object[] attrs = member.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                            if (attrs.Length > 0)
                                ap.description = (attrs[0] as System.ComponentModel.DescriptionAttribute).Description;
                            else
                                ap.description = "默认权限";

                            result.Add(ap);
                        }

                    }
                }
            }
            return result;
        }
    }
}
