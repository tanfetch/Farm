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
    public class ReportModel
    {
        public string reportName { get; set; }
        public string displayName { get; set; }

        public IQueryable dataSource { get; set; }
    }

    public class ReportController : BaseController
    {
        [Description("报表")]
        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        [Description("调入报表")]
        public ActionResult Grant()
        {
            return PartialView();
        }

        [HttpPost]
        [Description("调入报表")]
        public ActionResult Grant(FormCollection fc)
        {
            DynamicQuery<Pig> smodel = new DynamicQuery<Pig>();
            smodel.UpdateModel(fc);
            var dataSource = new FarmRepository().GetEntities<Pig>(smodel.whereExp);

            ReportModel report = new ReportModel();
            report.dataSource = dataSource;
            report.reportName = "Grant.rdlc";
            report.displayName = "调入报表";

            Session["report"] = report;

            return JavaScript(JSHelper.ShowSuccess("正在生成报表…"));
        }

        [HttpGet]
        [Description("销售报表")]
        public ActionResult Sales()
        {
            return PartialView();
        }

        [HttpPost]
        [Description("销售报表")]
        public ActionResult Sales(FormCollection fc)
        {
            DynamicQuery<Sales> smodel = new DynamicQuery<Sales>();
            smodel.UpdateModel(Request.Form);
            var dataSource = new FarmRepository().GetEntities<Sales>(smodel.whereExp);

            ReportModel report = new ReportModel();
            report.dataSource = dataSource;
            report.reportName = "Sales.rdlc";
            report.displayName = "销售报表";

            Session["report"] = report;

            return JavaScript(JSHelper.ShowSuccess("正在生成报表…"));
        }

        [HttpGet]
        [Description("成活率排名报表")]
        public ActionResult Sort()
        {
            return PartialView();
        }

        [HttpPost]
        [Description("成活率排名报表")]
        public ActionResult Sort(FormCollection fc)
        {
            DynamicQuery<Pig> smodel = new DynamicQuery<Pig>();
            smodel.UpdateModel(Request.Form);
            var dataSource = new FarmRepository().GetEntities<Pig>(smodel.whereExp);

            ReportModel report = new ReportModel();
            report.dataSource = dataSource;
            report.reportName = Request["sortby"] + ".rdlc";
            report.displayName = "成活率排名";

            Session["report"] = report;

            return JavaScript(JSHelper.ShowSuccess("正在生成报表…"));
        }
    }
}

