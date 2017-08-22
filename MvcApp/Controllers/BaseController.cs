using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Farm.AppCommon;
using Farm.Authority.Filter;
using Farm.Authority.DataContext;
using System.Linq.Expressions;

namespace Farm.Controllers
{
    [AuthorizeFilter]
    public abstract class BaseController : AsyncController
    {

        [ChildActionOnly]
        protected ActionResult Entity<T, TI>(BaseRepository<TI> repository,int id, string show = "Details")
            where T : class,TI
            where TI : IBaseTable
        {
            var model = repository.GetEntitie<T>(p=>p.ID==id);
            if (model == null)
                return HttpNotFound();

            //return Json(model, JsonRequestBehavior.AllowGet);
            return PartialView(show, model);
        }

        [ChildActionOnly]
        protected ActionResult Entity<T, TI>(BaseRepository<TI> repository, string show = "Details") 
            where T : class,TI 
            where TI: IBaseTable
        {
            var dynamicQuery = new DynamicQuery<T>();
            dynamicQuery.UpdateModel(Request.Form);
            dynamicQuery.UpdateModel(Request.QueryString);

            var model = repository.GetEntitie<T>(dynamicQuery.whereExp );
            if (model == null)
                return HttpNotFound();
            
            //return Json(model, JsonRequestBehavior.AllowGet);
            return PartialView(show, model);
        }

        [ChildActionOnly]
        protected IQueryable<T> Entities<T, TI>(BaseRepository<TI> repository)
            where T : class,TI
            where TI : IBaseTable
        {
            var dynamicQuery = new DynamicQuery<T>();
            dynamicQuery.UpdateModel(Request.Form);
            dynamicQuery.UpdateModel(Request.QueryString);

            var model = repository.GetEntities<T>(dynamicQuery.whereExp);

            return model;
        }
        
        [ChildActionOnly]
        protected ActionResult DataGrid<T,TI>(BaseRepository<TI> repository) 
            where T: class,TI
            where TI: IBaseTable
        {
            int p,r;
            Int32.TryParse(Request["page"],out p);
            Int32.TryParse(Request["rows"], out r);
            p = p < 1 ? 1 : p;
            r = r < 1 ? 15 : r;
            string sort = Request["sort"];
            string order = Request["order"];

            var dynamicQuery = new DynamicQuery<T>();
            dynamicQuery.UpdateModel(Request.Form);
            dynamicQuery.UpdateModel(Request.QueryString);

            int total = 0;
            var model = repository.GetEntities<T>(out total, dynamicQuery.whereExp, sort, order, p, r);

            var js = new
            {
                total = total,
                rows = model
            };

            return Json(js, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        protected ActionResult DataGrid<T>(IQueryable<T> source) where T :class
        {
            int p = 1;
            int r = 20;
            Int32.TryParse(Request["page"], out p);
            Int32.TryParse(Request["rows"], out r);
            string sort = Request["sort"];
            string order = Request["order"];

            var dynamicQuery = new DynamicQuery<T>();
            dynamicQuery.UpdateModel(Request.Form);
            dynamicQuery.UpdateModel(Request.QueryString);

            int total = 0;
            var model = source.Entities(out total,dynamicQuery.whereExp, sort, order, p, r);

            var js = new
            {
                total = total,
                rows = model
            };

            return Json(js, JsonRequestBehavior.AllowGet);
        }


        [ChildActionOnly]
        protected ActionResult UpdateEntity<T>() where T: class,IBaseTable,new()
        {
            T model = new T();
            TryUpdateModel(model);

            string result="";
            if (!ModelState.IsValid)
            {
                result = "验证失败：" + ModelState.Values.First(p=>p.Errors.Count>0).Errors.First().ErrorMessage;
                return Json(JSHelper.JsonMessage(result, false), JsonRequestBehavior.AllowGet);
            }

            BaseRepository repository = new BaseRepository();
            result = repository.Update<T>(model);

            if(!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result,false),JsonRequestBehavior.AllowGet);

            return Json(JSHelper.JsonMessage("操作成功", true), JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        protected ActionResult UpdateProperty<T>(int id, string property, object value) where T : class,IBaseTable, new()
        {
            string result = "";
            BaseRepository repository = new BaseRepository();
            result = repository.UpdateProperty<T>(p => p.ID == id, property, value);

            if (!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result, false), JsonRequestBehavior.AllowGet);

            return Json(JSHelper.JsonMessage("操作成功", true), JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        protected ActionResult DeleteEntity<T>(int id) where T : class,IBaseTable
        {
            BaseRepository repository = new BaseRepository();
            var result = repository.Delete<T>(p => p.ID == id);

            if (!string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result, false), JsonRequestBehavior.AllowGet);

            return Json(JSHelper.JsonMessage("操作成功", true), JsonRequestBehavior.AllowGet);
        }

    }
}
