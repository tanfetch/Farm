using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Farm.Raisers.DataContext
{
    public class tbSalesMetadata
    {
        [DisplayName("销售日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime salesDate { get; set; }

        [DisplayName("销售数量")]
        [Range(1, int.MaxValue)]
        public int salesNum { get; set; }

        [DisplayName("买主")]
        [Required]
        public string customer { get; set; }

        [DisplayName("销售员")]
        [Required]
        public string salesperson { get; set; }

        [DisplayName("销售方式")]
        [Required]
        public string salesmethods { get; set; }

        [DisplayName("等级")]
        [Required]
        public string grade { get; set; }

        [DisplayName("发运重量")]
        [Range(1, int.MaxValue)]
        public float salesWeight { get; set; }

        [DisplayName("到岸重量")]
        [Range(1, int.MaxValue)]
        public float deliveryWeight { get; set; }

        [DisplayName("单价")]
        [Range(0, int.MaxValue)]
        public float price { get; set; }

        [DisplayName("备注")]
        public string description { get; set; }
    }
}
