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
    public partial class tbFeedGrant : BaseTable,IBaseTable
    {
        #region 扩展属性

        public string raiserID { get; set; }

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

        #endregion

        #region 扩展方法
        partial void OnCreated()
        {

            
        }

        partial void OnValidate(ChangeAction action)
        {
            base.Validate(action);
        }

        override protected void OnInsertValidate()
        {
            var db = new BaseRepository();
            var r = db.GetEntitie<LivePig>(p => p.raiserID == this.raiserID);
            if (r == null)
                throw(new Exception( string.Format("养户编号\"{0}\"不存在", raiserID) ) );

            if (db.GetEntitie<tbFeedGrant>(p => p.PigID == r.ID && p.checkState == 0) != null)
                throw(new Exception( string.Format("养户\"{0}\"有未审核的发料记录", raiserID) ));

            this.PigID = r.ID;
            this.fromDays = r.feedGrantToDays + 1;
            this.realNum = r.extantNum;
            this.checkState = (r.feedSurplusDays + this.delayDays + this.addDays) > AppGlobal.grantFeedDay ? 0 : 1;

            var f = FeedHelper.GetFeeds(this.fromDays, this.fromDays + this.addDays - 1, r.extantNum);
            this.feeds = FeedHelper.GetRegexStr(f);

            this.referPerson = Account.currentUser == null? "" : Account.currentUser.userName;
            this.checkPerson = this.referPerson;
            this.referTime = DateTime.Now;
            this.checkTime = DateTime.Now;

        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {
            if (!canDel)
                throw (new Exception(string.Format("不能撤消以前的发料记录！")));
            return;
        }

        #endregion

        
    }
}
