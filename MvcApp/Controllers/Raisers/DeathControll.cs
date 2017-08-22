using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using Farm.Controllers;
using Farm.Authority.Filter;
using Farm.Raisers.DataContext;
using Farm.Raisers;
using System.ComponentModel;
using Farm.AppCommon;

namespace Farm.Controllers.Raisers
{
    public class DeathController : BaseController
    {
        [ChildActionOnly]
        private ActionResult Statistics()
        {
            FarmRepository rps = new FarmRepository();
            var model = base.Entities<Death, IFarmTable>(rps);

            return PartialView("Statistics", model);
        }

        [HttpGet]
        [Description("查看死亡记录")]
        public ActionResult List(string t)
        {
            if (t == "Statistics")
                return Statistics();

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
        [Description("修改死亡记录")]
        public ActionResult Edit(int id)
        {
            return base.Entity<tbDeath, IBaseTable>(new BaseRepository<IBaseTable>(), id, "Edit");
        }

        [HttpPost]
        [Description("修改死亡记录")]
        public ActionResult Edit(FormCollection fc)
        {
            return base.UpdateEntity<tbDeath>();
        }

        [HttpGet]
        [Description("录入死亡记录")]
        public ActionResult Update(string view = "Create")
        {
            return PartialView(view); ;
        }

        [HttpPost]
        [Description("录入死亡记录")]
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

