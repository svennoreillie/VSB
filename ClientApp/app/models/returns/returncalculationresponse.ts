import { ReturnCalculationType, ReturnCalculationLine } from "../index";


export class ReturnCalculationResponse {
    public returnLines: ReturnCalculationLine[];
    public amountRefundableByOGM: number;
    public amountNonRefundable: number;
    public totalAmount: number;
    public amountRefundableByDeduction: number;
    public type: ReturnCalculationType;

    constructor() {
        this.returnLines = new Array<ReturnCalculationLine>();
    }
}
