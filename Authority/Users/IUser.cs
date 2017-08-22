using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Farm.Authority.Permission;
using Farm.AppCommon;

namespace Farm.Authority.Users
{
    public interface IUser : IBaseTable
    {
        int ID {get;set;}
        string userID { get; set; }
        string userName { get; set; }
        string userPassword { get; set; }

        int logTimes { get; set; }
        string lastLogIP { get; set; }
        DateTime lastLogTime { get; set; }

        List<IActionPermission> actionPermissions { get; }
    }
}
