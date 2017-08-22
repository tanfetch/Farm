using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Farm.Raisers.DataContext
{
    public class tbPactMetadata
    {
        [DisplayName("编号")]
        public int ID { get; set; }

        [DisplayName("签约日期")]
        [Required()]
        public DateTime pactDate { get; set; }

        [DisplayName("签约数量")]
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage="签约数量不确定")]
        public int pactNum { get; set; }

    }
}
