using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.AppCommon
{

    public static class AppGlobal
    {
        static public int grantFeedDay { get { return 7; } }

        //计算orgDate早endDate的天数
        public static int DateDiff(DateTime orgDate, DateTime endDate)
        {
            TimeSpan ts1 = new TimeSpan(orgDate.Ticks);
            TimeSpan ts2 = new TimeSpan(endDate.Ticks);
            TimeSpan ts = ts2.Subtract(ts1);
            return ts.Days;
        }
        
    }
}
