using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using Farm.Raisers;
using System.Linq.Expressions;
using Farm.Raisers.Feeds;

namespace Farm.Raisers.DataContext
{
    public partial class SettledBatch : BaseTable, IFarmTable
    {
        static BaseRepository db = new BaseRepository();
        #region 扩展属性
       
        public double salesWeight
        {
            get
            {
                return Sales.Sum(p => p.salesWeight);
            }
        }

        public tbCheckout checkout
        {
            get
            {
                var checkout = db.GetEntitie<tbCheckout>(p => p.PigID == this.ID);
                return checkout == null ? new tbCheckout() : checkout;
            }
        }


        private List<tbSales> Sales
        {
            get
            {
                return db.GetEntities<tbSales>(p => p.PigID == this.ID).OrderBy(p => p.salesDate).ToList();
            }
        }
        private List<tbDeath> Death
        {
            get
            {
                return db.GetEntities<tbDeath>(p => p.PigID == this.ID).OrderBy(p => p.deathDate).ToList();
            }
        }
        private List<tbInjection> injection
        {
            get
            {
                return db.GetEntities<tbInjection>(p => p.PigID == this.ID).OrderBy(p => p.injectionDate).ToList();
            }
        }

        private List<tbFeedGrant> feeds
        {
            get
            {
                return db.GetEntities<tbFeedGrant>(p => p.PigID == this.ID).OrderBy(p => p.referTime).ToList();
            }
        }
        #endregion

        #region 扩展方法

        public tbSales GetSale(int index)
        {
            if (index < Sales.Count())
                return Sales.ElementAt(index);
            else 
                return null;
        }
        public tbDeath GetDeath(int index)
        {
            if (index < Death.Count())
                return Death.ElementAt(index);
            else
                return null;
        }
        public tbInjection GetInjection(int index)
        {
            if (index < injection.Count())
                return injection.ElementAt(index);
            else
                return null;
        }
  
        public string GetFeed(string name, int index)
        {
            List<string> result = new List<string>();
            feeds.ForEach(a =>
            {
                List<Feed> fd = FeedHelper.GetFeeds(a.feeds);
                DateTime dt = a.referTime;
                double nv = 0;
                fd.ForEach(f =>
                {
                    if (f.name == name)
                    {
                        nv += f.nValue * 40;
                    }
                });
                if (nv > 0)
                {
                    string s = string.Format("{0:MM-dd} / {1}", dt, nv);
                    result.Add(s);
                }
            });

            if (result.Count <= index) return "";
            return result.ElementAt(index);
        }

        public double GetFeedWeight(string name)
        {
            double nv = 0;
            feeds.ForEach(a =>
            {
                List<Feed> fd = FeedHelper.GetFeeds(a.feeds);
                fd.ForEach(f =>
                {
                    if (f.name == name)
                    {
                        nv += f.nValue * 40;
                    }
                });
            });
            return nv;
        }
        
        #endregion


    }
}
