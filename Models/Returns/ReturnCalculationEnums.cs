using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public enum ReturnCalculationType {
        NothingToReturn = 0,
        FullDeduction = 1,
        PartialDeduction = 2,
        NonDeductable = 3
    }

    public enum ReturnCalculationKind {
        ZVZ = 1,
        BOB = 2,
        THAB = 3
    }

}