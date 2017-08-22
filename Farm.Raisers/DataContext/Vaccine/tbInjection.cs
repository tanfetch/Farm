using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Farm.AppCommon;
using Farm.Raisers;

namespace Farm.Raisers.DataContext
{
    public partial class tbInjection : BaseTable,IBaseTable
    {

        static BaseRepository db = new BaseRepository();
        #region 扩展属性

        public string raiserID { get; set; }

        private tbVaccine vaccine
        {
            get
            {
                return db.GetEntitie<tbVaccine>(p => p.ID == this.vaccineID);
            }
        }

        public DateTime planDate
        {
            get
            {
                var pig = db.GetEntitie<tbPig>(p => p.ID == this.PigID);
                if (pig == null) return new DateTime(2000, 1, 1);
                return pig.grantDate.AddDays(vaccine.day);
            }
        }

        public string vaccineName
        {
            get
            {
                if (vaccine == null) return "";
                return vaccine.name;
            }
        }

        #endregion

        #region 扩展方法
        partial void OnCreated()
        {


        }

        partial void OnValidate(ChangeAction action)
        {
            base.Validate(action);
        }

        override protected void OnInsertValidate()
        {
            var r = new FarmRepository().GetEntitie<LivePig>(p => p.raiserID == raiserID);
            if (r == null)
                throw (new Exception(string.Format("养户编号\"{0}\"不存在", raiserID)));

            if (r.grantDate > this.injectionDate)
                throw (new Exception( string.Format("注射日期不可能早于调入日期:{0:d}", r.grantDate) ));

            if (DateTime.Today < this.injectionDate)
                throw (new Exception( string.Format("今天是{0:d},请不要提前确认", DateTime.Today) ));

            var a = new BaseRepository().GetEntitie<tbInjection>(p => p.PigID == r.ID && p.vaccineID == vaccineID);
            if (a != null)
                throw (new Exception(  string.Format("不用重复确认同一种疫苗") ));

            this.PigID = r.ID;
            return;
        }

        override protected void OnUpdateValidate()
        {

            return;
        }

        override protected void OnDeleteValidate()
        {
            return;
        }

        #endregion


    }
}
