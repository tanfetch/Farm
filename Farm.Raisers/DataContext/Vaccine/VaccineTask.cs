using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Users;
using Farm.Authority.Permission;
using Farm.AppCommon;
using System.Data.Linq;
using Farm.Raisers;
using Farm.Raisers.Vaccines;
using System.Linq.Expressions;

namespace Farm.Raisers.DataContext
{
    public partial class VaccineTask : BaseTable, IFarmTable
    {
        #region 扩展属性

        /*
        public List<Vaccine> vaccines {
            get {
                var ls = GetVaccinePlan();
                return ls.Where(p => !p.realyInjectionDate.HasValue).ToList();
            }
        }
   */

        

        public bool hasOverdue {
            get {
                return injectDate.Date < DateTime.Today.Date;
            }
        }

        private int planID
        {
            get 
            {
                var db = new BaseRepository();
                var plans = db.GetEntities<tbVaccinePlan>(p => !p.disabled && p.applyDate < this.grantDate);
                if (plans.Count() == 0)
                    return 0;

                return plans.OrderByDescending(p=>p.applyDate).First().ID;
            }
        }

        #endregion

        #region 扩展方法

        public List<Vaccine> GetVaccinePlan()
        {
            List<Vaccine> rePlan = new List<Vaccine>();

            List<tbVaccine> plan = new BaseRepository()
                .GetEntities<tbVaccine>(p => p.planID == this.planID).OrderBy(p => p.day).ToList();

            if (plan.Count == 0)
                return rePlan;

            var realy = new BaseRepository()
                .GetEntities<tbInjection>(p => p.PigID == this.ID && p.tbVaccine.planID == this.planID);

            foreach (var p in plan)
            {
                Vaccine v = new Vaccine();
                v.ID = p.ID;
                v.name = p.name;
                v.planInjectionDate = this.grantDate.AddDays(p.day);

                var r = realy.FirstOrDefault(o => o.vaccineID == p.ID);
                v.realyInjectionDate = (r == null ? null : (DateTime?)r.injectionDate);
                v.injectionID = (r == null ? -1 : r.ID);
                rePlan.Add(v);
            }

            DateTime[] planDate = plan.Select(p => this.grantDate.AddDays(p.day)).ToArray();
            var T1 = rePlan.Where(p => p.realyInjectionDate.HasValue).OrderBy(o => o.realyInjectionDate).ToList();
            var T2 = rePlan.Where(p => !p.realyInjectionDate.HasValue).OrderBy(o => o.planInjectionDate).ToList();

            rePlan = T1.Concat(T2).ToList();
            rePlan[0].reviseInjectionDate = planDate[0];
            for (int i = 1; i < rePlan.Count; i++)
            {
                int delay = 0;
                if (rePlan[i - 1].realyInjectionDate.HasValue)
                    delay = AppGlobal.DateDiff(planDate[i - 1], rePlan[i - 1].realyInjectionDate.Value);
                else
                    delay = AppGlobal.DateDiff(planDate[i - 1], rePlan[i - 1].reviseInjectionDate);

                rePlan[i].reviseInjectionDate = planDate[i].AddDays(delay);
            }

            return rePlan;

        }

        #endregion

        
    }
}
