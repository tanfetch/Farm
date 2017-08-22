using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Farm.Authority.Users;
using Farm.Authority.DataContext;
using Farm.AppCommon;

namespace Farm.Authority.Filter
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            if (this.AuthorizeCore(filterContext) == true)//根据验证判断进行处理
                return;

            string responseText = Account.currentUser==null ? "由于长时间没有操作,需要重新登录": "抱歉,你没有当前操作的权限！" ;

            string responseType = filterContext.HttpContext.Request["rspType"];
            filterContext.Result = NoAuthorize(responseType, responseText);

            return;
        }

        //权限判断业务逻辑
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var user = Account.currentUser;//获取当前用户信息

            if (user == null)
                return false;

            var actions = user.actionPermissions;

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();

            var actionPermission = actions.FirstOrDefault(p => p.controllerName.ToLower() == controllerName.ToLower() && p.actionName.ToLower() == actionName.ToLower());

           
            return actionPermission != null;
        }

        protected ActionResult NoAuthorize(string responseType, string responseText)
        {
            ActionResult result;

            if(!string.IsNullOrEmpty(responseType))
                responseType = responseType.ToLower();

            switch (responseType)
            {
                case "json":
                    result =
                        new JsonResult { Data = JSHelper.JsonMessage(responseText,false,0), 
                            JsonRequestBehavior=JsonRequestBehavior.AllowGet };
                    break;
                case "script":
                    result =
                        new JavaScriptResult { Script = JSHelper.ShowError(responseText)  };
                    break;
                case "content":
                    result =
                        new ContentResult { Content = string.Format("{0}", responseText) };
                    break;

                case "view":
                    result = new RedirectResult(string.Format("/Logon/Error?text={0}",responseText));
                        //new PartialViewResult { ViewName = "", ViewData = new ViewDataDictionary(new {text = responseText }) };
                    break;

                default:
                    result =
                        new JsonResult
                        {
                            Data = JSHelper.JsonMessage(responseText, false, 0),
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                        //new ContentResult { Content = responseText };
                        //new ContentResult { Content = string.Format("{0},{1}", controllerName, actionName) };
                    break;
            }

            return result;
        }
    }
}
