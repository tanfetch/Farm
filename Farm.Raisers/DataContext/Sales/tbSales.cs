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
    [MetadataType(typeof(tbSalesMetadata))]
    public partial class tbSales : BaseTable, IBaseTable
    {
        #region 扩展属性

        public string raiserID { get; set; }
        public bool canDel
        {
            get
            {
                return new BaseRepository().GetEntitie<tbCheckout>(p => p.PigID == this.PigID) == null;
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
            var r = new FarmRepository().GetEntitie<LivePig>(p => p.raiserID == raiserID);
            if (r == null)
                throw (new Exception(string.Format("养户编号\"{0}\"不存在", raiserID)));
            if (r.extantNum < this.salesNum)
                throw (new Exception(string.Format("销售数量不能大于当前存栏数量")));

            this.PigID = r.ID;
            return;
        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {
            if (!this.canDel)
                throw (new Exception(string.Format("不能删除已结算批次的销售记录")));
            return;
        }

        #endregion

        
    }
}
