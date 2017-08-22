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
using MvcApp.Models.Reports;
using Farm.Authority.Users;
using System.Web.SessionState;
using System.Threading.Tasks;

namespace Farm.Controllers.Raisers
{

    [SessionState(SessionStateBehavior.ReadOnly)]
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

        [HttpGet]
        [Description("生产报表")]
        public ActionResult Production()
        {
            return PartialView();
        }

        [HttpPost]
        [Description("生产报表")]
        public ActionResult Production(DateTime orgDate, DateTime endDate)
        {
            IEnumerable<ProductionReportResult> dataSource;

            using (var dc = new FarmDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                int userID = Account.userID;
                dataSource = dc.ProductionReport(userID, orgDate, endDate).ToList();
            }

            ReportModel report = new ReportModel();
            report.dataSource = dataSource.AsQueryable();
            report.reportName = "Production.rdlc";
            report.displayName = "生产报表";

            report.Parameters.Add(new Microsoft.Reporting.WebForms.ReportParameter("orgDate", orgDate.ToShortDateString()));
            report.Parameters.Add(new Microsoft.Reporting.WebForms.ReportParameter("endDate", endDate.ToShortDateString()));
            Session["report"] = report;
            
            
            return JavaScript(JSHelper.ShowSuccess("正在生成报表…"));
        }
    }
}

