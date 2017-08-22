using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using Farm.Raisers;

namespace Farm.Raisers.DataContext
{
    public partial class Checkout : BaseTable, IFarmTable
    {
        #region 扩展属性
        private Pig thePig
        {
            get
            {
                var pig = new BaseRepository().GetEntitie<Pig>(p => p.ID == this.PigID);
                return pig != null ? pig : new Pig();
            }
        }

        public int batch
        {
            get
            {
                return thePig.batch;
            }
        }

        public int salesNum
        {
            get
            {
                return thePig.salesNum;
            }
        }

        public double salesWeightByNum
        {
            get
            {
                if (thePig.salesNum == 0) return 0;
                return thePig.salesWeight / thePig.salesNum;
            }
        }
        public int feedDays
        {
            get
            {
                return thePig.feedDays;
            }
        }
        public double liveRate
        {
            get
            {
                return (double)thePig.liveRate;
            }
        }

        #endregion

        #region 扩展方法
       

        #endregion

        
    }
}
