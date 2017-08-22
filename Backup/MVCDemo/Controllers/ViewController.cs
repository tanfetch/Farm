using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farm.Raisers.DataContext;

namespace MVCDemo.Controllers
{
    public class ViewController : Controller
    {
        //
        // GET: /View/

        public JsonResult AutoComplete(string id)
        {
            FarmDataContext dc = new FarmDataContext();
            if (string.IsNullOrEmpty(id))
                Response.End();

            IEnumerable<string> xx = null;
            switch (id)
            {
                case "fwd":
                    //xx = (from x in Field.GetFieldIE() select x).Distinct();
                    break;

                case "source":
                    xx = (from x in dc.tbPig select x.source).Distinct();
                    break;

                case "yhname":
                    xx = (from x in dc.tbRaiser select x.name).Distinct();
                    break;

                case "yhaddress":
                    xx = (from x in dc.tbRaiser select x.address).Distinct();
                    break;

                case "transportMan":
                    xx = dc.tbPig.Select(p => p.transportMan).Distinct();
                    break;

                case "purchaseMan":
                    xx = dc.tbPig.Select(p => p.purchaseMan).Distinct();
                    break;

                case "technician":
                    xx = dc.tbPig.Select(p => p.technician).Distinct();
                    break;

                case "xsy":
                    xx = dc.tbSales.Select(p => p.salesperson).Distinct();
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

    }
}
