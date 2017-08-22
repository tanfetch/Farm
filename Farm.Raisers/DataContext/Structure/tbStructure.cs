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
    public partial class tbStructure : BaseTable,IBaseTable
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

            return;
        }

        #endregion

        
    }
}
