using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using Farm.Raisers;
using Farm.Raisers.Feeds;

namespace Farm.Raisers.DataContext
{
    public partial class GrantFeed : BaseTable, IFarmTable
    {
        #region 扩展属性
        public bool canDel
        {
            get
            {
                var db = new BaseRepository();
                if (db.GetEntitie<tbFeedGrant>(p => p.PigID == this.PigID && p.fromDays > this.fromDays) != null)
                    return false;
                
                return db.GetEntitie<tbCheckout>(p => p.PigID == this.PigID) == null;
            }
        }

        public string feedsDetail
        {
            get
            {
                var f = FeedHelper.GetFeeds(this.feeds);
                return FeedHelper.GetFeedsDetail(f, 0);
            }
        }

        #endregion

        #region 扩展方法

        /// <summary>
        /// 发料详细字符串
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public string GetFeedsDetail(int flag = 0)
        {
            var l = FeedHelper.GetFeeds(feeds);
            return FeedHelper.GetFeedsDetail(l, flag);
        }

        /// <summary>
        /// 发料详细列表
        /// </summary>
        /// <returns></returns>
        public List<Feed> GetFeedsList()
        {
            List<Feed> fs = new List<Feed>();
            fs.AddRange(FeedHelper.GetFeeds(feeds));
            return fs;
        }

        /// <summary>
        /// 体重增量
        /// </summary>
        /// <returns></returns>
        public double WeightInc()
        {
            List<Feed> feed = FeedHelper.GetFeeds(this.feeds);
            if (feed.Count == 0)
                return 0.0;

            return feed.Sum(p => p.weight);
        }

        /// <summary>
        /// 本次发料食至标准日龄的增重
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public double WeightInc(int days)
        {
            if (days < this.fromDays)
                return 0;

            if (days > (fromDays + addDays - 1))
                return WeightInc();

            List<Feed> feeds = FeedHelper.GetFeeds(this.fromDays, days, 1);
            if (feeds.Count == 0)
                return 0.0;

            return feeds.Sum(p => p.weight);
        }

        /// <summary>
        /// 每日增重
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public double WeightIncPreDay(int days)
        {
            if (days < this.fromDays || days > this.fromDays + this.addDays - 1)
                return 0;

            List<Feed> feeds = FeedHelper.GetFeeds(days, days, 1);

            if (feeds.Count == 0)
                return 0.0;

            return feeds.Sum(p => p.weight);
        }

        /// <summary>
        /// 按标准日龄每天饲料用量
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public string FeedByDay(int from, int to)
        {
            if (from < this.fromDays)
                from = this.fromDays;
            if (to > this.fromDays + this.addDays - 1)
                to = this.fromDays + this.addDays - 1;

            var num = this.realNum;
            List<Feed> feeds = FeedHelper.GetFeeds(from, to, num);
            if (feeds.Count == 0)
                return string.Empty;

            var s = FeedHelper.GetFeedsDetail(feeds, 1);

            return s;
        }

        /// <summary>
        /// 不同种类饲料的重量和
        /// </summary>
        /// <returns></returns>
        public double GetFeedWeight()
        {
            List<Feed> feed = FeedHelper.GetFeeds(this.feeds);
            if (feed.Count == 0)
                return 0.0;

            return feed.Sum(p => p.wValue);
        }

        public double GetFeedWeight(int orgDayOld, int endDayOld)
        {
            if (endDayOld < this.realFeedDays || orgDayOld > this.realFeedDays + this.addDays)
                return 0;

            var w = GetFeedWeight();
            if (orgDayOld < this.realFeedDays && endDayOld > this.realFeedDays + this.addDays)
                return w;

            return w * (endDayOld - orgDayOld + 1)/this.addDays;

            /*
            int from = this.realFeedDays > orgDayOld ? 
                this.fromDays : this.fromDays + orgDayOld - this.realFeedDays;
            int to = endDayOld - this.realFeedDays < this.addDays ? 
                this.fromDays + endDayOld - this.realFeedDays : this.fromDays + this.addDays;

            List<Feed> feed = FeedHelper.GetFeeds(from, to-1, this.realNum);
            if (feed.Count == 0)
                return 0;
            */

            //return feed.Sum(p => p.wValue);
        }

        /// <summary>
        /// 不同种类饲料的包数和
        /// </summary>
        /// <returns></returns>
        public double GetFeedBagNum()
        {
            List<Feed> feed = FeedHelper.GetFeeds(this.feeds);
            if (feed.Count == 0)
                return 0.0;

            return feed.Sum(p => p.nValue);
        }

        public static double GetBagNum(DateTime dt1, DateTime dt2)
        {
            var dc = new BaseRepository();
            var fs = dc.GetEntities<GrantFeed>(p =>dt1<=p.referTime.Date && p.referTime.Date <= dt2);

            return fs.ToList().Sum(p => p.GetFeedBagNum());
        }

        

        #endregion

        
    }
}
