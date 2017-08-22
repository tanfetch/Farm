using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using Farm.Controllers;

using Farm.Raisers.DataContext;
using Farm.Raisers;
using System.ComponentModel;

namespace Farm.Controllers.Raisers
{
    public class PigController : BaseController
    {
        [HttpGet]
        [Description("查看在养列表")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看在养列表")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<LivePig, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("调入和修改调入信息")]
        public ActionResult Update()
        {
            return PartialView("Create");
        }

        [HttpPost]
        [Description("调入和修改调入信息")]
        public ActionResult Update(FormCollection fc)
        {
            return base.UpdateEntity<tbPig>();
        }

        [Description("删除调入记录")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbPig>(id);
        }

    }

  
}


