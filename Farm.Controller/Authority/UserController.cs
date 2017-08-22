using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Farm.Authority.Users;

using Farm.Authority.Permission;
using System.Reflection;
using Farm.Authority.DataContext;
using System.ComponentModel;
using Farm.AppCommon;

namespace Farm.Controllers.Authority
{

    public class UserController : BaseController
    {

        [HttpGet]
        [Description("查看用户列表")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看用户列表")]
        public ActionResult List(FormCollection fc)
        {
            AuthorityRepository rps = new AuthorityRepository();
            return base.DataGrid<Users, IBaseTable>(rps);
        }

        [HttpGet]
        [Description("修改用户信息")]
        public ActionResult Update(string go)
        {
            if (go == "Edit")
            {
                int id;
                Int32.TryParse(Request["id"], out id);
                return base.Entity<tbUser, IBaseTable>(new AuthorityRepository(), id, "Edit");
            }

            return PartialView("Create");
        }

        [HttpPost]
        [Description("修改用户信息")]
        public ActionResult Update(int ID,FormCollection fc)
        {
            string purview = Request["pur"];

            tbUser.UpdatePurview(ID, purview);
  
            return base.UpdateEntity<tbUser>();
        }

        [Description("删除用户信息")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbUser>(id);
        }

    }
}
