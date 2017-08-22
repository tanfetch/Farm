using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farm.Raisers.DataContext;
using Farm.AppCommon;
using MvcApp.Models.Reports;

namespace MvcApp.Controllers
{
    public class JSController : Controller
    {
        //
        // GET: /View/

        public JsonResult JsonList(string id)
        {
            BaseRepository dc = new BaseRepository();
            if (string.IsNullOrEmpty(id))
                Response.End();

            IEnumerable<string> xx = null;
            switch (id)
            {
                case "fwd":
                    //xx = (from x in Field.GetFieldIE() select x).Distinct();
                    break;

                case "source":
                    xx = dc.GetEntities<tbPig>().Select(p=>p.source).Distinct();
                    break;

                case "yhname":
                    xx = dc.GetEntities<tbRaiser>().Select(p=>p.name).Distinct();
                    break;

                case "yhaddress":
                    xx = dc.GetEntities<tbRaiser>().Select(p => p.address).Distinct();
                    break;

                case "transportMan":
                    xx = dc.GetEntities<tbPig>().Select(p=>p.transportMan).Distinct();
                    break;

                case "purchaseMan":
                    xx = dc.GetEntities<tbPig>().Select(p => p.purchaseMan).Distinct();
                    break;

                case "technician":
                    xx = dc.GetEntities<tbPig>().Select(p => p.technician).Distinct();
                    break;

                case "salesperson":
                    xx = GetSalespersons();
                    break;

                case "customer":
                    xx = dc.GetEntities<tbSales>().Select(p => p.customer).Distinct();
                    break;

                default:
                    xx = null;
                    break;
            }

            var output = new System.Text.StringBuilder();
            if (xx != null)
            {
                foreach (var m in xx)
                {
                    var s = m;
                    output.AppendFormat("{0},", s);
                }
            }

            return Json(output.ToString().Split(','), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<string> GetSalespersons()
        { 
            BaseRepository dc = new BaseRepository();
            var ls = dc.GetEntities<tbSales>().Select(p => p.salesperson).Distinct();

            var result = new List<string>();
            foreach (var item in ls)
            {
                var a = item
                    .Replace(' ',';')
                    .Replace('　', ';')
                    .Replace("；", ";")
                    .Replace("，", ";")
                    .Replace("、", ";")
                    .Replace("。", ";")
                    .Replace("：", ";");

                var x = a.Split(new char[] { ';', ',', '.', ' ' }).FirstOrDefault();
                if(!string.IsNullOrEmpty(x))
                    result.Add(x);
            }

            return result.Distinct();
        }

        public ActionResult Sort(FormCollection fc)
        {
            DynamicQuery<Pig> smodel = new DynamicQuery<Pig>();
            smodel.UpdateModel(Request.Form);
            var dataSource = new FarmRepository().GetEntities<Pig>(smodel.whereExp).ToList().AsQueryable();

            ReportModel report = new ReportModel();
            report.dataSource = dataSource;
            report.reportName = "Area.rdlc";
            report.displayName = "成活率排名";

            Session["report"] = report;

            return JavaScript(JSHelper.ShowSuccess("正在生成报表…"));
        }
    }
}
