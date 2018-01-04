using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public class ReturnCalculationPayment {
        public decimal Amount { get; set; }
        public int UnCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SendDate { get; set; }
    }
}