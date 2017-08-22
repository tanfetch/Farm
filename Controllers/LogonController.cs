using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Farm.Authority.Users;
using Farm.Authority.Filter;
using Farm.Authority.Permission;
using System.Reflection;
using Farm.Authority.DataContext;
using System.ComponentModel;
using Farm.AppCommon;

namespace Farm.Controllers.Authority
{

    public class LogonController : Controller
    {
        [HttpPost]
        [Description("用户登录")]
        public ActionResult Index(FormCollection fn)
        {
            LogonModel logon = new LogonModel();
            UpdateModel<LogonModel>(logon);
            if (!ModelState.IsValid)
                Json(JSHelper.JsonMessage("非法登录", false));

            logon.logIP = Request.UserHostAddress.ToString();

            var result = Account.Logon(logon);
            if (!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result));

            return Json(JSHelper.JsonMessage("登录成功,请稍候……",true));
        
        }

        public ActionResult Index()
        {
            Account.Logout();
            return View();
        }


 



    }
}
