using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations;

namespace Farm.Authority.DataContext
{
    [MetadataType(typeof(tbRoleMetadata))]
    public partial class tbRole :BaseTable, IBaseTable
    {
        #region 扩展属性
        public int userNum {
            get {
                var db = new AuthorityRepository();
                return db.GetEntities<tbUser>(p => p.roleID == this.ID).Count();
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
            var db = new AuthorityRepository();
            if (db.GetEntities<tbRole>(p => p.name == this.name).Count() > 0)
                throw (new Exception(string.Format("权限组名 \"{0}\" 已经存在，请更换", this.name)));
            return;
        }

        override protected void OnUpdateValidate()
        {
            var db = new AuthorityRepository();
            if (db.GetEntities<tbRole>(p => p.name == this.name && p.ID!=this.ID).Count() > 0)
                throw (new Exception(string.Format("权限组名 \"{0}\" 已经存在，请更换", this.name)));
            return;
        }

        override protected void OnDeleteValidate()
        {
            var db = new AuthorityRepository();
            if (db.GetEntities<tbUser>(p => p.roleID == this.ID).Count() > 0)
                throw (new Exception(string.Format("权限正在使用,不能删除", this.name)));

            db.Delete<tbPermission>(p => p.roleID == this.ID);
            return;
        }

        #endregion
    }
}
