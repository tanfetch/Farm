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
    [MetadataType(typeof(tbUserMetadata))]
    public partial class tbUser :BaseTable, IUser
    {
        #region 扩展属性
        public List<IActionPermission> actionPermissions
        {
            get
            {
                var conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                AuthorityDataContext dc = new AuthorityDataContext(conn);

                var actions = from a in dc.tbPermission
                              join b in dc.tbRole on a.roleID equals b.ID
                              join c in dc.tbUser on b.ID equals c.roleID
                              where c.ID == this.ID
                              select (IActionPermission)a;

                return actions.ToList();

            }
        }

        public string[] pur { get; set; }

        public string purviewStr
        {
            get
            {
                var db = new BaseRepository().GetEntities<Purview>(p => p.userID == this.ID);
                string s = "";
                foreach (var m in db)
                {
                    if (s != "")
                        s = s + ",";
                    s = s + m.areaID;
                }
                return s;
            }
 
        }

        public string roleName 
        { 
            get 
            {
                var db = new AuthorityRepository();
                var role = db.GetEntitie<tbRole>(p => p.ID == this.roleID);
                if(role!=null)
                    return role.name;
                return string.Empty;
            } 
        }

        #endregion

        #region 扩展方法
        partial void OnCreated()
        {
            this.lastLogTime = DateTime.Now;
            this.lastLogIP = "";
            
        }

        partial void OnValidate(ChangeAction action)
        {
            base.Validate(action);
        }

        override protected void OnInsertValidate()
        {
            AuthorityRepository rps = new AuthorityRepository();
            if (rps.GetEntities<tbUser>(p => p.userID == this.userID).Count() > 0)
                throw (new Exception(string.Format("系统中已存在用户名\"{0}\",请使用其他名称", this.userID)));

            return;
        }

        protected override void OnUpdateValidate()
        {
            //this.UpdatePurview();
        }

        static public void UpdatePurview(int ID, string pur)
        {
            var db = new BaseRepository();
            db.Delete<Purview>(p => p.userID == ID);

            if (string.IsNullOrEmpty(pur))
                return;

            var areaID = pur.Split(',');

            foreach (string n in areaID)
            {
                Purview purview = new Purview();
                purview.userID = ID;
                purview.areaID = Int32.Parse(n);

                db.Inset<Purview>(purview);
            }
        }

        #endregion

        
    }
}
