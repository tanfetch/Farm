using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.AppCommon;

namespace Farm.Raisers
{
    public interface IFarmTable : IBaseTable
    {
        int areaID { get; set; }
    }
}
