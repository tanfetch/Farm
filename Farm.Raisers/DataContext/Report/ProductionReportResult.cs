using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.AppCommon;

namespace Farm.Raisers.DataContext
{
    public partial class ProductionReportResult : BaseTable, IFarmTable
    {
        #region 扩展属性

        public double feedCast
        {
            get
            {
                int orgDayOld = AppGlobal.DateDiff(this.grantDate, this.orgDate.Value) + this.grantDayOld;
                int endDayOld = AppGlobal.DateDiff(this.grantDate, this.endDate.Value) + this.grantDayOld;

                var dc = new FarmRepository();
                var fs = dc.GetEntities<GrantFeed>(p =>p.PigID==this.ID && p.referTime.Date < this.endDate.Value.AddDays(1).Date);
                if (fs.Count() == 0)
                    return 0;

                var reslut = fs.ToList().Sum(p=>p.GetFeedWeight(orgDayOld,endDayOld));

                return reslut;
            }
            set 
            {
            }
        }

        #endregion

        #region 扩展方法

        public List<ProductionReportResult> GetList()
        {
            return new List<ProductionReportResult>();
        }

        #endregion


    }
}
