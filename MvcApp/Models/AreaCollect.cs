using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class AreaGroup
    {
        public int areaID { get; set; }

        public string areaName { get; set; }

        public int raiserNum { get; set; }
        public int idleRaiserNum { get; set; }

        public int contractedRaiserNum { get; set; }
        public int liveBatchNum { get; set; }

        public double liveRateExtant { get; set; }
        public double liveRate { get; set; }

        public double ratio { get; set; }
        public double ratio32 { get; set; }
        public double ratio35 { get; set; }

        public int grantNum { get; set; }
        public int salesNum { get; set; }
        public int deathNum { get; set; }
        public int extantNum { get; set; }
    }
}