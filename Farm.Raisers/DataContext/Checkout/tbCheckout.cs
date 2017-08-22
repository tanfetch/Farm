using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using Farm.Raisers;
using System.ComponentModel.DataAnnotations;

namespace Farm.Raisers.DataContext
{
    [MetadataType(typeof(tbCheckoutMetadata))]
    public partial class tbCheckout : BaseTable,IBaseTable
    {
        #region 扩展属性
        public string raiserID { get; set; }
        public bool canDel
        {
            get
            {
                var db = new BaseRepository();
                var pig = db.GetEntitie<Pig>(p => p.ID == this.PigID);
                if (pig == null)
                    return true;

                var raiser = db.GetEntitie<Raiser>(p => p.ID == pig.RID);
                if (raiser == null)
                    return true;

                return pig.batch == raiser.totalBatch;
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
            var db = new FarmRepository();
            var r = db.GetEntitie<ClosurePig>(p => p.raiserID == raiserID);
            if (r == null)
                throw(new Exception( string.Format("养户编号\"{0}\"不存在", raiserID) ));
            if (db.GetEntitie<Checkout>(p => p.PigID == r.ID) != null)
                throw(new Exception( string.Format("已存在当前批次的结算记录") ));

            this.PigID = r.ID;
            this.referPerson = Account.userName;
            this.referTime = DateTime.Now;
            return;
        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {
            if (!this.canDel)
                throw (new Exception(string.Format("不能删除结算记录")));
            return;
        }

        #endregion

        
    }
}
