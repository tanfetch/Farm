using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Farm.Authority.DataContext
{
    public class tbUserMetadata
    {
        [Required]
        [DisplayName("帐户")]
        public string userID { get; set; }

        [Required]
        [DisplayName("姓名")]
        public string userName { get; set; }

        [Required]
        [DisplayName("密码")]
        public string userPassword { get; set; }

        [Required]
        [DisplayName("权限")]
        public int roleID { get; set; }
    }

    public class tbRoleMetadata
    {
        [Required]
        [DisplayName("权限名")]
        public string name { get; set; }

        [Required]
        [DisplayName("权限说明")]
        public string description { get; set; }
    }
}
