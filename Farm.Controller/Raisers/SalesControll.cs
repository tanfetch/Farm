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
    public class SalesController : BaseController
    {

        [HttpGet]
        [Description("查看销售记录")]
        public ActionResult List()
        {
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
        [Description("录入和修改销售记录")]
        public ActionResult Update()
        {
            return PartialView("Create");
        }

        [HttpGet]
        [Description("录入和修改销售记录")]
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

