using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Farm.Raisers.DataContext
{
    public class tbCheckoutMetadata
    {
        [Required]
        [DisplayName("结算金额")]
        public double amount { get; set; }

        [Required]
        [DisplayName("头平结算金额")]
        public double ArgAmount { get; set; }

        [Required]
        [DisplayName("合同结算金额")]
        public double pactAmount { get; set; }

        [Required]
        [DisplayName("全程料肉比")]
        public double ratioAll { get; set; }

        [Required]
        [DisplayName("3.2还原料肉比")]
        public double ratio { get; set; }

        [Required]
        [DisplayName("3.5还原料肉比")]
        public double ratio2 { get; set; }

        [Required]
        [DisplayName("寒暑津贴")]
        public double allowance { get; set; }

        [Required]
        [DisplayName("成活率奖")]
        public double bonus { get; set; }

        [Required]
        [DisplayName("药费")]
        public double drugCost { get; set; }

        [Required]
        [DisplayName("分值")]
        public double score { get; set; }

        [DisplayName("备注")]
        public string description { get; set; }
    }
}
