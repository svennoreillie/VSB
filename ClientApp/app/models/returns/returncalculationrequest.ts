import { ReturnCalculationLine } from "../index";

export class ReturnCalculationRequest {
    public siNumber: number;
    public returnCalculationId: number;
    public federationNumber: number;

    public isFraude: boolean;
    public isError: boolean;
    public reason: string;
    public otherReason: string;
    public remark: string;
    public insz: string;
    public createdBy: string;

    public returnByDeduction: number;
    public returnByOGM: number;
    
    public regularLetterCreateDate: Date;
    public regularLetterCreatedBy: string;
    public signedLetterCreateDate: Date;
    public signedLetterCreatedBy: string;

    public objectionMade: boolean;
    public objectionDate: Date;
    public returnStarted: boolean;

    public returnLines: ReturnCalculationLine[];

    constructor() {
        this.returnLines = new Array<ReturnCalculationLine>();
        this.isFraude = false;
    }

    
}
