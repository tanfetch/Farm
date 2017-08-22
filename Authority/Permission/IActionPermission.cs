using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.Authority.Permission
{
    public interface IActionPermission
    {
        string controllerName { get; set; }
        string actionName { get; set; }

    }

    public class actionPermission
    {
        string controllerName { get; set; }
        string actionName { get; set; }
 
    }
}
