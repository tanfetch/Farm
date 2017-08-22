using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Farm.Raisers.DataContext;

namespace MvcApp.Models.Reports
{
    public class Production : Pig
    {
        static FarmRepository dc = new FarmRepository();

        public DateTime orgDate { get; set; }
        public DateTime endDate { get; set; }

        public int orgExtantNum 
        {
            get
            {
                return this.grantNum
                    - dc.GetEntities<Sales>(p => p.PigID == this.ID && p.salesDate < orgDate).Sum(p=>p.salesNum)
                    - dc.GetEntities<Death>(p => p.PigID == this.ID && p.deathDate < orgDate).Sum(p=>p.deathNum);
            }
        }

        public int endExtantNum
        {
            get
            {
                return this.grantNum
                    - dc.GetEntities<Sales>(p => p.PigID == this.ID && p.salesDate < endDate).Sum(p => p.salesNum)
                    - dc.GetEntities<Death>(p => p.PigID == this.ID && p.deathDate < endDate).Sum(p => p.deathNum);
            }
        }

        public List<Production> GetList()
        {
            return new List<Production>();
        }

    }
}