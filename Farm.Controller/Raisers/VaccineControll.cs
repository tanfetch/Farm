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
using Farm.AppCommon;

namespace Farm.Controllers.Raisers
{
    public class VaccineController : BaseController
    {

        [ChildActionOnly]
        private ActionResult Preview()
        {
            string raiserID = Request["raiserID"];
            var db = new FarmRepository();
            var plan = db.GetEntitie<VaccinePlan>(p => p.raiserID == raiserID);
            if (plan == null)
                return Json(new {success=false },JsonRequestBehavior.AllowGet);

            var vacc = plan.GetVaccinePlan();

            var js = new
            {
                success = true,
                areaName = plan.areaName,
                raiserName = plan.raiserName,
                planName = plan.planName,
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
        public ActionResult Update(string raiserID, int vaccineID, DateTime realyInjectionDate)
        {
            var result = VaccinePlan.Injection(raiserID, vaccineID, realyInjectionDate);
            if (!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result,false),JsonRequestBehavior.AllowGet);
            return Json(JSHelper.JsonMessage("保存成功", true),JsonRequestBehavior.AllowGet);
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
            var plans = VaccinePlan.GetPlans().AsQueryable();
            return base.DataGrid(plans);
        }
    }
}

