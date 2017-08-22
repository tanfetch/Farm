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
using System.Web.SessionState;
using System.Threading.Tasks;

namespace Farm.Controllers.Raisers
{
    public class VaccineController : BaseController
    {

        [ChildActionOnly]
        private ActionResult Preview()
        {
            string raiserID = Request["raiserID"];
            var db = new FarmRepository();
            var plan = db.GetEntitie<VaccineTask>(p => p.raiserID == raiserID);
            if (plan == null)
                return Json(new {success=false },JsonRequestBehavior.AllowGet);

            var vacc = plan.GetVaccinePlan();

            var js = new
            {
                success = true,
                areaName = plan.areaName,
                raiserName = plan.raiserName,
                plan = vacc.Where(p=>!p.realyInjectionDate.HasValue),
                planAll = vacc
            };

            return Json(js, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [Description("确认疫苗注射")]
        public ActionResult Update()
        {
            string action = Request["go"];
            if (action == "preview")
                return Preview();

            return PartialView("Create");
        }

        [HttpPost]
        [Description("确认疫苗注射")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbInjection>();
        }

        [Description("删除疫苗确认记录")]
        public ActionResult UndoInjection(int id)
        {
            return base.DeleteEntity<tbInjection>(id);
        }

        [HttpGet]
        [Description("查看疫苗注射任务")]
        public ActionResult List()
        {
            return PartialView("List");
        }

       
        [HttpPost]
        [Description("查看疫苗注射任务")]
        public ActionResult List(FormCollection fc)
        {
            
            var db = new FarmRepository();
            var source = db.GetEntities<VaccineTask>(p => p.injectDate<new DateTime(2050,12,31));
            return base.DataGrid(source);
             
            //return base.DataGrid<VaccineTask,IFarmTable>(new FarmRepository());
        }
    }
}

