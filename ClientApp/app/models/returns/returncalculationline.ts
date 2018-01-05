import { ReturnCalculationKind } from "../index";


export class ReturnCalculationLine {
    public kind: ReturnCalculationKind;
    public paymentLines: ReturnCalculationPayment[];

    constructor() {
        this.paymentLines = new Array<ReturnCalculationPayment>();
    }
}

export class ReturnCalculationPayment {
    constructor(public amount: number,
        public unCode: number,
        public startDate: Date,
        public endDate: Date,
        public sendDate: Date) {

    }
}