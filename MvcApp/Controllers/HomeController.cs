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
using System.Web.SessionState;
using Farm.Raisers.Feeds;
using System.Xml.Linq;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class AreaCollectModel 
    {
        public int areaID { get; set; }
        public string areaName { get; set; }
        public int value { get; set; }
    }

    [SessionState(SessionStateBehavior.ReadOnly)]
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

        public ActionResult Attention()
        {
            return PartialView();
        }

        public async Task<ActionResult> ContractedRaiserAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() => { 
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                var ls = db.GetEntities<Pact>(p => p.statusFlag == 0);
                List<AreaCollectModel> model = new List<AreaCollectModel>();
                foreach (var item in structures)
                {
                    var m = new AreaCollectModel
                    {
                        areaID = item.ID,
                        areaName = item.name,
                        value = ls.Count(p => p.areaID == item.ID)
                    };
                    model.Add(m);
                }

                return PartialView(model); ;
            });

            return reslut;
        }

        public async Task<ActionResult> FeedShortAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() => { 
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                var ls = db.GetEntities<LivePig>(p => p.feedSurplusDays < 1);
                List<AreaCollectModel> model = new List<AreaCollectModel>();
                foreach (var item in structures)
                {
                    var m = new AreaCollectModel
                    {
                        areaID = item.ID,
                        areaName = item.name,
                        value = ls.Count(p => p.areaID == item.ID)
                    };
                    model.Add(m);
                }

                //return model;
                return PartialView(model);
            });

            //return PartialView(reslut);
            return reslut;
        }

        public async Task<ActionResult> FeedExceedAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                var ls = db.GetEntities<LivePig>(p=>p.grantDate.Date < DateTime.Today.AddDays(-150).Date);

                List<AreaCollectModel> model = new List<AreaCollectModel>();
                foreach (var item in structures)
                {
                    var m = new AreaCollectModel
                    {
                        areaID = item.ID,
                        areaName = item.name,
                        value = ls.Count(p => p.areaID == item.ID)
                    };
                    model.Add(m);
                }

                //return model;
                return PartialView(model);
            });

            //return PartialView(reslut);
            return reslut;
        }

        public async Task<ActionResult> ClosurePigAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                var ls = db.GetEntities<ClosurePig>();

                List<AreaCollectModel> model = new List<AreaCollectModel>();
                foreach (var item in structures)
                {
                    var m = new AreaCollectModel
                    {
                        areaID = item.ID,
                        areaName = item.name,
                        value = ls.Count(p => p.areaID == item.ID )
                    };
                    model.Add(m);
                }

                //return model;
                return PartialView(model);
            });

           // return PartialView(reslut);
            return reslut;
        }

        public async Task<ActionResult> VaccineShortAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                var ls = db.GetEntities<VaccineTask>().ToList();

                List<AreaCollectModel> model = new List<AreaCollectModel>();
                
                foreach (var item in structures)
                {
                    var m = new AreaCollectModel
                    {
                        areaID = item.ID,
                        areaName = item.name,
                        value = ls.Count(p=>p.areaID==item.ID && p.hasOverdue)
                    };
                    model.Add(m);
                }

                //return model;
                return PartialView(model);
            });

            //return PartialView(reslut);
            return reslut;
        }

        //签约超过25天的合同
        public async Task<ActionResult> PactOverTimeAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new FarmRepository();

                var model = db.GetEntities<Pact>(p =>p.statusFlag==0 && p.pactDate < DateTime.Today.AddDays(-25));
                //return model;
                return PartialView(model);
            });

            //return PartialView(reslut);
            return reslut;
        }

        //清栏超过25天的批次
        public async Task<ActionResult> CloseOverTimeAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new FarmRepository();

                var model = db.GetEntities<ClosurePig>(p => p.closeDate < DateTime.Today.AddDays(-25));
                return model;
            });

            return PartialView(reslut);
        }

        //清栏超过2天没领料的养户
        public async Task<ActionResult> FeedOverTimeAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new FarmRepository();

                var model = db.GetEntities<LivePig>(p => p.feedSurplusDays < -2);
                return model;
            });

            return PartialView(reslut);
        }

        //成活率低于90%的养户
        public async Task<ActionResult> LiveRateShortAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new FarmRepository();

                var model = db.GetEntities<LivePig>(p => p.liveRate < (decimal)0.9);
                return model;
            });

            return PartialView(reslut);
        }


        //区域信息汇总
        public async Task<ActionResult> AreaGroupAsync(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();
                var structures = db.GetEntities<tbStructure>(p => p.typeID == 1);
                List<AreaGroup> model = new List<AreaGroup>();
                DateTime year = new DateTime(DateTime.Today.Year, 1, 1);

                foreach (var item in structures)
                {
                    var pig = db.GetEntities<Pig>(p => p.areaID == item.ID && p.grantDate >= year);
                    var livepig = db.GetEntities<LivePig>(p => p.areaID == item.ID);
                    var closedpig = db.GetEntities<ClosedPig>(p =>p.areaID == item.ID && p.checkoutDate >= year);

                    var raiser = db.GetEntities<Raiser>(p => p.areaID == item.ID);
                    var checkout = db.GetEntities<Checkout>(p => p.areaID == item.ID && p.referTime >= year);
                    var sales = db.GetEntities<Sales>(p => p.areaID == item.ID && p.salesDate >= year);
                    var death = db.GetEntities<Death>(p => p.areaID == item.ID && p.deathDate >= year);

                    var m = new AreaGroup();
                        
                    m.areaID = item.ID;
                    m.areaName = item.name;
                    m.raiserNum = raiser.Count();
                    m.idleRaiserNum = raiser.Where(p => p.statusFlag == 3).Count();
                    m.contractedRaiserNum = raiser.Where(p=>p.statusFlag == 1).Count();
                    m.liveBatchNum = livepig.Count();

                    m.liveRateExtant = livepig.Count() == 0? 0: (double)livepig.Average(p=>p.liveRate);
                    m.liveRate = closedpig.Count() == 0 ? 0 : (double)closedpig.Average(p => p.liveRate);

                    m.ratio = checkout.Count() == 0 ? 0 : checkout.Average(p => p.ratioAll);
                    m.ratio32 = checkout.Count() == 0 ? 0 : checkout.Average(p => p.ratio);
                    m.ratio35 = checkout.Count() == 0 ? 0 : checkout.Average(p => p.ratio2);

                    m.grantNum = pig.Count()==0? 0 : pig.Sum(p=>p.grantNum);
                    m.salesNum = sales.Count()==0? 0 : sales.Sum(p => p.salesNum);
                    m.deathNum = death.Count()==0? 0 : death.Sum(p => p.deathNum);
                    m.extantNum = livepig.Count()==0? 0 :livepig.Sum(p => p.extantNum);

                    model.Add(m);
                }

                return model;
            });

            var js = new
            {
                total = reslut.Count,
                rows = reslut,
                footer = new[]{ 
                    new{
                        areaName = "合计",
                        raiserNum = reslut.Sum(p => p.raiserNum),
                        idleRaiserNum = reslut.Sum(p => p.idleRaiserNum),

                        contractedRaiserNum = reslut.Sum(p => p.contractedRaiserNum),
                        liveBatchNum = reslut.Sum(p => p.liveBatchNum),

                        liveRateExtant = reslut.Count(p=>p.liveRateExtant>0)==0 ? 0 : 
                                        reslut.Where(p=>p.liveRateExtant>0).Average(p=>p.liveRateExtant),
                        liveRate = reslut.Count(p=>p.liveRate>0)==0 ? 0 : reslut.Where(p=>p.liveRate>0).Average(p => p.liveRate),
                        ratio = reslut.Count(p=>p.ratio>0) == 0 ? 0 : reslut.Where(p=>p.ratio>0).Average(p => p.ratio),
                        ratio32 = reslut.Count(p=>p.ratio32>0) == 0 ? 0 : reslut.Where(p=>p.ratio32>0).Average(p => p.ratio32),
                        ratio35 = reslut.Count(p=>p.ratio35>0) == 0 ? 0 : reslut.Where(p=>p.ratio35>0).Average(p => p.ratio35),

                        grantNum = reslut.Sum(p => p.grantNum),
                        salesNum = reslut.Sum(p => p.salesNum),
                        deathNum = reslut.Sum(p => p.deathNum),
                        extantNum = reslut.Sum(p => p.extantNum)

                        ,iconCls = "icon-sum"
                    }
                }
            };

            return Json(js, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Summary1Async(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();

                DateTime year = new DateTime(DateTime.Today.Year, 1, 1);

                ViewDataDictionary model = new ViewDataDictionary();
                model["养户总数"] = db.GetCount<tbRaiser>(p=>true);
                model["签约待调"] = db.GetCount<Pact>(p => p.statusFlag == 0);
                model["空闲养户数"] = db.GetCount<Raiser>(p => p.statusFlag == 3);
                model["在养数"] = db.GetCount<LivePig>(p => true);
                model["待结数"] = db.GetCount<ClosurePig>(p => true);

                return model;
            });

            ViewData = reslut;
            return PartialView();
        }

        public async Task<ActionResult> Summary2Async(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();

                DateTime year = new DateTime(DateTime.Today.Year, 1, 1);

                ViewDataDictionary model = new ViewDataDictionary();

                model["今年调入头数"] = db.GetCount<tbPig>(p => p.grantDate >= year) == 0 ? 0 :
                    db.GetEntities<tbPig>(p => p.grantDate >= year).Sum(p => p.grantNum);

                model["今年销售头数"] = db.GetCount<tbSales>(p => p.salesDate >= year) == 0 ? 0 :
                    db.GetEntities<tbSales>(p => p.salesDate >= year).Sum(p => p.salesNum);

                model["今年死亡头数"] = db.GetCount<tbDeath>(p => p.deathDate >= year) == 0 ? 0 :
                    db.GetEntities<tbDeath>(p => p.deathDate >= year).Sum(p => p.deathNum);

                model["批次成活率"] = db.GetCount<ClosedPig>(p => p.checkoutDate >= year) == 0 ? 0 :
                    db.GetEntities<ClosedPig>(p => p.checkoutDate >= year).Average(p => p.liveRate);

                model["在养成活率"] = db.GetCount<LivePig>(p => true) == 0 ? 0 :
                    db.GetEntities<LivePig>(p => true).Average(p => p.liveRate);

                model["3.5还原料肉比"] = db.GetCount<Checkout>(p => p.referTime >= year) == 0 ? 0 :
                    db.GetEntities<Checkout>(p => p.referTime >= year).Average(p => p.ratio2);
                model["全程料肉比"] = db.GetCount<Checkout>(p => p.referTime >= year) == 0 ? 0 :
                    db.GetEntities<Checkout>(p => p.referTime >= year).Average(p => p.ratioAll);

                return model;
            });

            ViewData = reslut;
            return PartialView();
        }

        public async Task<ActionResult> Summary3Async(string view)
        {
            var reslut = await Task.Factory.StartNew(() =>
            {
                var db = new BaseRepository();

                DateTime year = new DateTime(DateTime.Today.Year, 1, 1);

                ViewDataDictionary model = new ViewDataDictionary();
              

                model["今天调入头数"] = db.GetCount<tbPig>(p => p.grantDate == DateTime.Today) == 0 ? 0 :
                    db.GetEntities<tbPig>(p => p.grantDate == DateTime.Today).Sum(p => p.grantNum);

                model["今天销售头数"] = db.GetCount<tbSales>(p => p.salesDate == DateTime.Today) == 0 ? 0 :
                    db.GetEntities<tbSales>(p => p.salesDate == DateTime.Today).Sum(p => p.salesNum);

                model["今天死亡头数"] = db.GetCount<tbDeath>(p => p.deathDate == DateTime.Today) == 0 ? 0 :
                    db.GetEntities<tbDeath>(p => p.deathDate == DateTime.Today).Sum(p => p.deathNum);

                model["今天发料包数"] = GrantFeed.GetBagNum(DateTime.Today, DateTime.Today);

                model["今天疫苗注射户数"] = db.GetCount<VaccineTask>(p => p.injectDate.Date == DateTime.Today.Date);
                
                return model;

            });

            ViewData = reslut;
            return PartialView();

        }

 

     
    }
}
