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
    public partial class LivePig : BaseTable, IFarmTable
    {
        #region 扩展属性
        public bool canDel
        {
            get
            {
                var db = new BaseRepository();
                if (db.GetEntitie<tbFeedGrant>(p => p.PigID == this.ID) != null)
                    return false;
                if (db.GetEntitie<tbDeath>(p => p.PigID == this.ID) != null)
                    return false;
                if (db.GetEntitie<tbSales>(p => p.PigID == this.ID) != null)
                    return false;
                if (db.GetEntitie<tbCheckout>(p => p.PigID == this.ID) != null)
                    return false;

                return true;
            }
        }

        #endregion

        #region 扩展方法

        public int GetLastGrantFeedDays()
        {
            var db = new BaseRepository();
            var a = db.GetEntities<tbFeedGrant>(p => p.PigID == this.ID)
                .OrderByDescending(p => p.fromDays).FirstOrDefault();
            if (a == null)
                return AppGlobal.grantFeedDay;

            return a.addDays;
        }

        /// <summary>
        /// 指定日期的日龄
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetDayOld(DateTime dt)
        {
            return this.grantDayOld + AppGlobal.DateDiff(this.grantDate, dt);
        }

        /// <summary>
        /// 日龄换算成日期
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public DateTime GetDateByDays(int days)
        {
            return this.grantDate.AddDays(days - this.grantDayOld);
        }

        /// <summary>
        /// 指定日龄的预计体重
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public double NormalWeight(int days)
        {
            double r;
            //如果日龄小于调入日龄，返回调入均重
            if (days < this.grantDayOld)
            {
                r = this.weightByNum;
                return r;
            }

            var db = new BaseRepository();
            var fs = db.GetEntities<GrantFeed>(p => p.PigID == this.ID && p.realFeedDays <= days
                && p.checkState > 0).ToList();

            //如果指定日龄前没有发料记录，返回调入均重
            if (fs.Count() == 0)
            {
                r = this.weightByNum;
                return r;
            }

            //如果日龄大于余料日龄，返回余料日龄时的均重
            if (days > this.feedUsedToDays)
            {
                r = fs.Sum(p => p.WeightInc()) + this.weightByNum;
                return r;
            }

            r = fs.Sum(p => p.WeightInc(days - p.delayDaysSum)) + this.weightByNum;

            return r;
        }

        #endregion

        
    }
}
