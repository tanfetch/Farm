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
    public partial class tbPig : BaseTable,IBaseTable
    {
        #region 扩展属性
        public string raiserID { get; set; }

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
            var b = db.GetEntitie<Pact>(p => p.raiserID == this.raiserID && p.statusFlag == 0);
            if (b == null)
                throw(new Exception( string.Format("养户编号\"{0}\"对应的合同信息不存在", this.raiserID)));

            this.RID = b.RID;
            this.PID = b.ID;
            return;
        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {
            if (!this.canDel)
                throw (new Exception("当前记录不允许删除"));
            return;
        }

        #endregion

        
    }
}
