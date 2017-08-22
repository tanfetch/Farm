using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farm.Authority.Filter;
using Farm.Authority.Users;
using Farm.Raisers.DataContext;
using Farm.AppCommon;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace MVCDemo.Controllers
{

    public class HomeController : AsyncController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            if (Account.currentUser == null)
                return Redirect("/Logon/");
            return View();
        }


        public ActionResult panel(string view)
        {
            var reslut = Article(view);
            return reslut.Result;
            //return PartialView(view);
        }

        public ActionResult panel1()
        {
            var reslut = Article("FeedShort");
            return reslut.Result;
        }

        public ActionResult panel2()
        {
            var reslut = Article("FeedExceed");
            return reslut.Result;
        }

        public ActionResult Test(string view)
        {
            var db = new BaseRepository();
            var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
            //var vaccineShort = VaccinePlan.GetPlans().ToList().Count(p => p.hasOverdue);
            var vs = db.GetEntities<VaccinePlan>().ToList();

            string s = "";
            foreach (var item in structures)
            {
                var text = item.name;
                var value = vs.Count(p => p.areaID == item.ID && p.hasOverdue );
                s = s+ text + ":" + value.ToString() + "." ;
            }
            ViewBag.text = s;
            
            return PartialView(view);
        }


        public Task<ActionResult> Article(string view)
        {
            return Task.Factory.StartNew(() =>
                {
                    ActionResult result = Test(view);
                    return result;
                })
                .ContinueWith<ActionResult>(task =>
                    {                    
                        return task.Result;
                        //return Content(string.Format("taskID:{0}",task.Id));
                    });
                 
        }

        public ActionResult temp(string view = "FeedExceed")
        {
            return Task.Factory.StartNew(() =>
            {
                ActionResult result = Test(view);
                return result;
            })
                .ContinueWith<ActionResult>(task =>
                {
                    return task.Result;
                    //return Content(string.Format("taskID:{0}",task.Id));
                }).Result;

        }

        public void Article2Async(string view)
        {
            AsyncManager.OutstandingOperations.Increment();
            Task.Factory.StartNew(() =>
                {
                    ActionResult result = Test(view);

                    AsyncManager.Parameters["content"] = result;
                   
                    AsyncManager.OutstandingOperations.Decrement();
                });
        }
        public ActionResult Article2Completed(ActionResult content)
        {
            return content;
        }


        public Task<ContentResult> News(string city)
        {
            return Task.Factory.StartNew(() => RunThread(city))
                .ContinueWith(t =>
                {
                    return Content(t.Result);
                });
        }
        private string RunThread(string input)
        {
            Thread.Sleep(5000);
            return input;
        }

    }
}
