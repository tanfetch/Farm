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
    public class SalesController : BaseController
    {
        [ChildActionOnly]
        private ActionResult Statistics()
        {
            FarmRepository rps = new FarmRepository();
            var model = base.Entities<Sales, IFarmTable>(rps);

            return PartialView("Statistics", model);
        }

        [HttpGet]
        [Description("查看销售记录")]
        public ActionResult List(string t)
        {
            if (t == "Statistics")
                return Statistics();

            return PartialView("List");
        }

        [HttpPost]
        [Description("查看销售记录")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<Sales, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("修改销售记录")]
        public ActionResult Edit(int id)
        {
            return base.Entity<tbSales, IBaseTable>(new BaseRepository<IBaseTable>(), id, "Edit");
        }

        [HttpPost]
        [Description("修改销售记录")]
        public ActionResult Edit(FormCollection fc)
        {
            return base.UpdateEntity<tbSales>();
        }

        [HttpGet]
        [Description("录入销售记录")]
        public ActionResult Update(string view="Create")
        {
            return PartialView(view);
        }

        [HttpPost]
        [Description("录入销售记录")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbSales>();
        }

        [Description("删除销售记录")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbSales>(id);
        }

        
    }
}

