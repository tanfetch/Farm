using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.DataContext;

namespace Farm.Authority.Users
{
    public class Account 
    {
        public static tbUser currentUser
        {
            get
            {
                var c = System.Web.HttpContext.Current;
                if(c != null)
                    return System.Web.HttpContext.Current.Session["user"] as tbUser;
                return new tbUser();
            }
            set
            {
                System.Web.HttpContext.Current.Session["user"] = value;
            }
        }

        public static int userID
        {
            get
            {
                if (Account.currentUser != null)
                    return Account.currentUser.ID;
                return 0;
            }
        }

        public static string userName {
            get {
                if (Account.currentUser != null)
                    return Account.currentUser.userName;
                return string.Empty;
            }
        }

        public static string Logon(LogonModel logon)
        {
            
            var db = new AuthorityRepository();

            tbUser user = db.GetEntitie<tbUser>(p => p.userID == logon.userID);
            if (user == null)
                return string.Format("登录失败：用户\"{0}\"不存在！", logon.userID);

            if (user.userPassword != logon.userPassword)
                return string.Format("登录失败：输入的密码不正确！");

            var role = db.GetEntitie<tbRole>(p => p.ID == user.roleID);
            if (role.disabled)
                return string.Format("登录失败：用户\"{0}\"所属的权限组已被停用！", logon.userID);
        
            user.lastLogIP = logon.logIP;
            user.lastLogTime = DateTime.Now;
            user.logTimes = user.logTimes + 1;

            Account.currentUser = user;

            db.Update((tbUser)user);

            return string.Empty;
        }

        public static void Logout()
        {
            Account.currentUser = null;
        }

        
    }
}
