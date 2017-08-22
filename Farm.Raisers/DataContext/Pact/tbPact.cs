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
    [MetadataType(typeof(tbPactMetadata))]
    public partial class tbPact : BaseTable, IBaseTable
    {
        #region 扩展属性
        public string raiserID { get; set; }

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
            var r = db.GetEntitie<Raiser>(p => p.raiserID == raiserID);
            if (r == null)
                throw(new Exception( string.Format("养户编号\"{0}\"不存在", raiserID) ));
            if (r.statusFlag != 3)
                throw(new Exception( string.Format("养户\"{0}\"处于非空闲状态，不能签约", raiserID) ));

            this.RID = r.ID;

            return;
        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {

            return;
        }

        #endregion

        
    }
}
