using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.AppCommon;
using System.Linq.Expressions;
using Farm.Authority.Users;
using Farm.Authority.DataContext;

namespace Farm.Raisers.DataContext
{
    public class FarmRepository : BaseRepository<IFarmTable> 
    {
        //System.Configuration.ConfigurationManager.ConnectionStrings["WellFulDB_V3_0ConnectionString"].ConnectionString
        /*
        public AuthorityRepository(): base (global::Farm.Authority.Properties.Settings.Default.TanDBConnectionString)
        {
            
        }
         */

        public FarmRepository()
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {

        }
         

        override protected IQueryable<T> EntitiesAll<T>()
        {
            FarmDataContext dc = new FarmDataContext(connection);
            dc.ObjectTrackingEnabled = false;

            var reslut = from a in dc.GetTable<T>()
                         join b in dc.GetTable<tbStructure>() on a.areaID equals b.ID
                         join c in dc.GetTable<Purview>() on b.ID equals c.areaID
                         where c.userID == Account.currentUser.ID
                         select a;

            return reslut;
        }


    }
}
