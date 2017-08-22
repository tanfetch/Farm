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
    public class DeathController : BaseController
    {


        [HttpGet]
        [Description("查看死亡记录")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看死亡记录")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<Death, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("录入和修改死亡记录")]
        public ActionResult Update()
        {
            return PartialView("Create"); ;
        }

        [HttpPost]
        [Description("录入和修改死亡记录")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbDeath>();
        }

        [Description("删除死亡记录")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbDeath>(id);
        }





    }
}

