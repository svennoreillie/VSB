﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public class ReturnCalculationLine {

        public ReturnCalculationLine(ReturnCalculationKind kind) {
            this.Kind = kind;
            this.PaymentLines = new List<ReturnCalculationPayment>();
        }

        public ReturnCalculationKind Kind { get; set; }
        public List<ReturnCalculationPayment> PaymentLines { get; set; }
    }
}