using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Farm.Raisers.DataContext
{
    public class tbDeathMetadata
    {

        [DisplayName("死亡日期")]
        [Required]
        public DateTime deathDate { get; set; }

        [DisplayName("死亡数量")]
        [Required]
        [Range(1, int.MaxValue)]
        public int deathNum { get; set; }

        [DisplayName("备注")]
        public string description { get; set; }
    }
}
