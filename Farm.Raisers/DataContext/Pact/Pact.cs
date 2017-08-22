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
    public partial class Pact : BaseTable, IFarmTable
    {
        #region 扩展属性

        public string raiserAddress
        {
            get 
            {
                var db = new FarmRepository();
                var r = db.GetEntitie<Raiser>(p => p.ID == this.RID);
                if (r == null)
                    return "";
                return r.address;
            }
        }

        public string raiserTelephone
        {
            get
            {
                var db = new FarmRepository();
                var r = db.GetEntitie<Raiser>(p => p.ID == this.RID);
                if (r == null)
                    return "";
                return r.telephone;
            }
        }

        #endregion

        #region 扩展方法
      

        #endregion

        
    }
}
