﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;
using Farm.Controllers;
using Farm.Authority.Filter;
using Farm.Raisers.DataContext;
using Farm.Raisers;
using System.ComponentModel;
using Farm.AppCommon;

namespace Farm.Controllers.Raisers
{
    public class PigController : BaseController
    {
        [ChildActionOnly]
        private ActionResult Statistics()
        {
            FarmRepository rps = new FarmRepository();
            var model = base.Entities<LivePig, IFarmTable>(rps);

            return PartialView("Statistics", model);
        }

        [HttpGet]
        [Description("查看在养列表")]
        public ActionResult List(string t)
        {
            if (t == "Statistics")
                return Statistics();

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
        [Description("修改调入信息")]
        public ActionResult Edit(int id)
        {
            return base.Entity<tbPig, IBaseTable>(new BaseRepository<IBaseTable>(), id, "Edit");
        }

        [HttpPost]
        [Description("修改调入信息")]
        public ActionResult Edit(FormCollection fc)
        {
            return base.UpdateEntity<tbPig>();
        }

        [HttpGet]
        [Description("调入")]
        public ActionResult Update()
        {
            return PartialView("Create");
        }

        [HttpPost]
        [Description("调入")]
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


