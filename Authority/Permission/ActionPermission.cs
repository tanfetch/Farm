using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.Authority.Permission
{
    public class ActionPermission : IActionPermission
    {
        public int roleID { get; set; }
        public bool isValid { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }

        public string description { get; set; }


        public string actionMark { get { return string.Format("{0}.{1}", controllerName, actionName); } }
    }
}
