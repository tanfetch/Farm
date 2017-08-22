using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.AppCommon;
using System.Linq.Expressions;
using Farm.Authority.Users;

namespace Farm.Authority.DataContext
{
    public class AuthorityRepository  :  BaseRepository<IBaseTable> 
    {
        //System.Configuration.ConfigurationManager.ConnectionStrings["WellFulDB_V3_0ConnectionString"].ConnectionString
        /*
        public AuthorityRepository(): base (global::Farm.Authority.Properties.Settings.Default.TanDBConnectionString)
        {
            
        }
         */
        
        public AuthorityRepository()
            : base(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString)
        {

        }
         

        override protected IQueryable<T> EntitiesAll<T>()
        {
            AuthorityDataContext dc = new AuthorityDataContext(connection);
            dc.ObjectTrackingEnabled = false;
            return dc.GetTable<T>();
        }

        public void xxx()
        { 
            
        }

    }
}
