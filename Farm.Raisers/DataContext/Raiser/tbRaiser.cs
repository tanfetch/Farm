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
    public partial class tbRaiser : BaseTable, IBaseTable
    {
        #region 扩展属性


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
            if (db.GetEntitie<tbRaiser>(p => p.raiserID == this.raiserID) != null)
                throw (new Exception(string.Format("养户编号 \"{0}\" 已经存在，请更换", this.raiserID)));
            return;
        }

        override protected void OnUpdateValidate()
        {
            var db = new FarmRepository();
            var model = db.GetEntitie<Raiser>(p => p.ID == this.ID);
            if (model.statusFlag != 3 && model.statusFlag != 0 && this.isDisabled)
                throw(new Exception( "不能淘汰非空闲的养户" ));

            if (db.GetEntitie<Raiser>(p => p.raiserID == this.raiserID && p.ID != this.ID) != null)
                throw(new Exception( string.Format("养户编号 \"{0}\" 已经存在，请更换", this.raiserID) ));

            return;
        }

        override protected void OnDeleteValidate()
        {

            return;
        }

        #endregion

        
    }
}
