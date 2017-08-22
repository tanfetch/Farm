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

namespace Farm.Controllers.Raisers
{
    public class PactController : BaseController
    {
        [ChildActionOnly]
        private ActionResult Statistics()
        {
            FarmRepository rps = new FarmRepository();
            var model = base.Entities<Pact, IFarmTable>(rps);

            return PartialView("Statistics", model);
        }

        [HttpGet]
        [Description("查看合同记录")]
        public ActionResult List()
        {
            string action = Request["t"];
            if (action == "Statistics")
                return Statistics();

            return PartialView("List");
        }

        [HttpPost]
        [Description("查看合同记录")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<Pact, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("录入签约记录,修改合同信息")]
        public ActionResult Update(string go)
        {
            if (go == "Edit")
            {
                int id;
                Int32.TryParse(Request["id"], out id);
                return base.Entity<Pact, IFarmTable>(new FarmRepository(), id, "Edit");
            }

            return PartialView("Create");
        }

        [HttpPost]
        [Description("录入签约记录,修改合同信息")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbPact>();
        }

        [Description("删除合同信息")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbPact>(id);
        }

        
        
    }
}

