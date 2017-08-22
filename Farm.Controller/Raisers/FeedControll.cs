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
using Farm.Raisers.Feeds;
using Farm.AppCommon;

namespace Farm.Controllers.Raisers
{
    public class FeedController : BaseController
    {

        [HttpGet]
        [Description("查看发料记录")]
        public ActionResult List()
        {
            return PartialView("List");
        }

        [HttpPost]
        [Description("查看发料记录")]
        public ActionResult List(FormCollection fc)
        {
            FarmRepository rps = new FarmRepository();
            return base.DataGrid<GrantFeed, IFarmTable>(rps);
        }

        [HttpGet]
        [Description("发料")]
        public ActionResult Update()
        {
            return PartialView("Create");
        }

        [HttpPost]
        [Description("发料")]
        public ActionResult Update(FormCollection fc)
        {
            string action = Request["go"];
            if (action == "preview")
                return FeedPreview();

            return base.UpdateEntity<tbFeedGrant>();
        }

        [ChildActionOnly]
        private ActionResult FeedPreview()
        {
            string raiserID = Request["raiserID"];
            var pig = new FarmRepository().GetEntitie<LivePig>(p => p.raiserID == raiserID);
            if (pig == null)
                return Json(new { success = false,message= string.Format("养户编号 \"{0}\" 没有对应的在养批次",raiserID) }, 
                    JsonRequestBehavior.AllowGet);

            string addDays = Request["addDays"];
            string delayDays = Request["delayDays"];

            int n2,n3;
            if (!Int32.TryParse(addDays, out n2))
                n2 = pig.GetLastGrantFeedDays();

            if (!Int32.TryParse(delayDays, out n3))
                n3 = pig.feedSurplusDays < 0 ? 0 - pig.feedSurplusDays : 0;

            int n1 = pig.feedGrantToDays + 1;
            //int n2 = addDays.HasValue ? addDays.Value : pig.GetLastGrantFeedDays();
            //int n3 = delayDays.HasValue ? delayDays.Value : (pig.feedSurplusDays < 0 ? 0 - pig.feedSurplusDays : 0);
            int n4 = pig.extantNum;

            var f = FeedHelper.GetFeeds(n1, n1 + n2 - 1, n4);

            var ylts = pig.feedSurplusDays + n2 + n3;
            var ylrq = DateTime.Today.AddDays(ylts);
            var model = new
            {
                success = true,
                raiserName = pig.raiserName,
                areaName = pig.areaName,
                from = n1,
                add = n2,
                delay = n3,
                num = n4,
                feeds = f,
                check = (ylts <= AppGlobal.grantFeedDay),
                feedinfo = string.Format("可用至{0:d}，余料天数{1} ", ylrq, ylts)
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Description("撤消发料记录")]
        public ActionResult Delete(int id)
        {
            return base.DeleteEntity<tbFeedGrant>(id);
        }

        [Description("打印发料表")]
        public ActionResult FeedReport(int? id,string raiserID)
        {
            var db = new FarmRepository();
            GrantFeed model = null;
            if (id.HasValue)
                model = db.GetEntitie<GrantFeed>(p => p.ID == id.Value);
            else
                model = db.GetEntities<GrantFeed>(p => p.raiserID == raiserID).OrderByDescending(p => p.referTime).FirstOrDefault();

            if (model == null)
                return HttpNotFound();

            return View("GrantReport",model);
        }

        [Description("审核发料记录")]
        public ActionResult Check(int id, int value)
        {
            var result = new BaseRepository().UpdateProperty<tbFeedGrant>(p => p.ID == id, "checkState", value);
            if ( !string.IsNullOrEmpty(result))
                return Json(JSHelper.JsonMessage(result),JsonRequestBehavior.AllowGet);

            return Json(JSHelper.JsonMessage("操作完成",true), JsonRequestBehavior.AllowGet);

        }

    }
}

