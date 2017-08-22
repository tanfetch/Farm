using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace MvcApp.Models.Reports
{
    public class ReportModel
    {
        public ReportModel()
        {
            Parameters = new List<ReportParameter>();
        }

        public string reportName { get; set; }
        public string displayName { get; set; }

        public IQueryable dataSource { get; set; }

        public List<ReportParameter> Parameters { get; set; }
    }
}