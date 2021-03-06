﻿using System;
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
    public partial class Sales : BaseTable, IFarmTable
    {
        #region 扩展属性

        public bool canDel
        {
            get
            {
                return new BaseRepository().GetEntitie<tbCheckout>(p => p.PigID == this.PigID) == null;
            }
        }

        public string technician
        {
            get
            {
                var dc = new BaseRepository();
                var a = dc.GetEntitie<tbPig>(p => p.ID == this.PigID);
                return a.technician;
            }
        }
        #endregion

        #region 扩展方法
       

        #endregion

        
    }
}
