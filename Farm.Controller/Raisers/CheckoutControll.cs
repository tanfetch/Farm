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
    public class CheckoutController : BaseController
    {

        [Description("查看结算详细信息")]
        public ActionResult Details()
        {
            FarmRepository rps = new FarmRepository();
            return base.Entity<Checkout, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("查看结算记录")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看结算记录")]
        public ActionResult List(FormCollection fn)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<Checkout, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("录入和修改结算信息")]
        public ActionResult Update()
        {
            return PartialView("Create"); ;
        }

        [HttpPost]
        [Description("录入和修改结算信息")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbCheckout>();
        }

        [Description("删除结算信息")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbCheckout>(id);
        }






    }
}

