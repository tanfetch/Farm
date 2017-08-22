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
        [Description("修改结算记录")]
        public ActionResult Edit(int id)
        {
            return base.Entity<tbCheckout, IBaseTable>(new BaseRepository<IBaseTable>(), id, "Edit");
        }

        [HttpPost]
        [Description("修改结算记录")]
        public ActionResult Edit(FormCollection fc)
        {
            return base.UpdateEntity<tbCheckout>();
        }

        [HttpGet]
        [Description("录入结算信息")]
        public ActionResult Update(string view = "Create")
        {
            return PartialView(view); ;
        }

        [HttpPost]
        [Description("录入结算信息")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbCheckout>();
        }

        [Description("删除结算信息")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbCheckout>(id);
        }

        [HttpGet]
        [Description("待结列表")]
        public ActionResult ClosurePig(string t)
        {
            if (t == "ClosurePigStatistics")
            {
                FarmRepository rps = new FarmRepository();
                var model = base.Entities<ClosurePig, IFarmTable>(rps);
                return PartialView("ClosurePigStatistics", model);
            }

            return PartialView("ClosurePig");
        }

        [HttpPost]
        [Description("待结列表")]
        public ActionResult ClosurePig(FormCollection fn)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<ClosurePig, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("查看历史记录")]
        public ActionResult Settle(int? id, string a)
        {
            if( !id.HasValue )
            {
                if(string.IsNullOrEmpty(a))
                {
                    return PartialView("SettleSearch");
                }
                else
                {
                    return PartialView("Settle");
                }
            }
            else
            {
                ViewBag.userName = Farm.Authority.Users.Account.userName;
                FarmRepository rps = new FarmRepository();
                var model = rps.GetEntitie<SettledBatch>(p=>p.ID == id);
                if( model == null ) model = new SettledBatch();
                return View("SettleDetails",model);
            }
            
        }

        [HttpPost]
        [Description("查看历史记录")]
        public ActionResult Settle(FormCollection fn)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<SettledBatch, IFarmTable>(rps);
        }

    }
}

