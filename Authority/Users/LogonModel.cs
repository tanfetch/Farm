using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.Authority.Users
{
    public class LogonModel
    {
        public string userID { get; set; }
        public string userPassword { get; set; }

        public string logIP { get; set; }
    }
}
