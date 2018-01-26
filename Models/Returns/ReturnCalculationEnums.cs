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
        BOB = 1,
        ZVZ = 2,
        THAB = 3,
        THAB_FOD = 4,
    }

}