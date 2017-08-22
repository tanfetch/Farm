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
using Farm.Raisers.DataContext;

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

        public ActionResult ChangePassword()
        {
            var p0 = Request["userPassword0"];
            if(string.IsNullOrEmpty(p0))
                return Json(JSHelper.JsonMessage("修改请输入原来的密码"));

            var p1 = Request["userPassword1"];
            var p2 = Request["userPassword2"];

            if (string.IsNullOrEmpty(p1) || string.IsNullOrEmpty(p2))
                return Json(JSHelper.JsonMessage("新密码不能为空"));

            if (!p1.Equals(p2))
                return Json(JSHelper.JsonMessage("两次输入的新密码不一致"));

            var db = new BaseRepository();
            var user = Account.currentUser;
            if(user == null)
                return Json(JSHelper.JsonMessage("网页已经过期,请重新登录"));

            if(user.userPassword != p0)
                return Json(JSHelper.JsonMessage("原密码不正确"));

            var u = db.GetEntitie<tbUser>(p => p.ID == user.ID);
            u.userPassword = p1;
            var result = db.Update<tbUser>(u);
            if(!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result));

            return Json(JSHelper.JsonMessage("操作成功", true));
        }


        public ActionResult Error(string text)
        {
            ViewBag.text = text;
            return PartialView("Error");
        }



        public ActionResult test()
        {
            BaseRepository repository = new BaseRepository();
            int p, r;
            Int32.TryParse(Request["page"], out p);
            Int32.TryParse(Request["rows"], out r);
            p = p < 1 ? 1 : p;
            r = r < 1 ? 15 : r;
            string sort = Request["sort"];
            string order = Request["order"];

            var dynamicQuery = new DynamicQuery<GrantFeed>();
            dynamicQuery.UpdateModel(Request.Form);
            dynamicQuery.UpdateModel(Request.QueryString);

            int total = 0;
            var model = repository.GetEntities<GrantFeed>(out total, dynamicQuery.whereExp, sort, order, p, r);

            var js = new
            {
                total = total,
                rows = model
            };

            return Json(js, JsonRequestBehavior.AllowGet);
        }

    }
}
