using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using Farm.Controllers;

using Farm.Raisers.DataContext;
using Farm.Raisers;
using System.ComponentModel;

namespace Farm.Controllers.Raisers
{
    public class RaiserController : BaseController
    {

        [HttpGet]
        [Description("查看养户列表")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看养户列表")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<Raiser, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("添加养户,修改养户信息")]
        public ActionResult Update(string go)
        {
            if (go == "Edit")
            {
                int id;
                Int32.TryParse(Request["id"],out id);
                return base.Entity<Raiser, IFarmTable>(new FarmRepository(), id, "Edit");
            }

            return PartialView("Create");
        }

        [HttpPost]
        [Description("添加养户,修改养户信息")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbRaiser>();
        }

        [Description("删除养户信息")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbRaiser>(id);
        }

        [Description("淘汰养户或恢复养户状态")]
        public ActionResult ToggleInvalid(int id, bool value)
        {
            return base.UpdateProperty<tbRaiser>(id, "isDisabled", value);
        }

    }
}

