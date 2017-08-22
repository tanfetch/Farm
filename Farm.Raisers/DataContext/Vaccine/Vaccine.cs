using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farm.Raisers.Vaccines
{
    public interface IVaccine
    {
        //string name { get; set; }
        //DateTime injectionDate { get; set; }
        int ID { get; set; }
    }

    public class Vaccine
    {
        public int ID { get; set; }
        public string name { get; set; }

        /// <summary>
        /// 方案中的应注射日期
        /// </summary>
        public DateTime planInjectionDate { get; set; }

        /// <summary>
        /// 根据方案和之前注射记录计算出来的应注射日期
        /// </summary>
        public DateTime reviseInjectionDate { get; set; }

        /// <summary>
        /// 实际注射日期
        /// </summary>
        public DateTime? realyInjectionDate { get; set; }

        public int injectionID { get; set; }

    }
}
