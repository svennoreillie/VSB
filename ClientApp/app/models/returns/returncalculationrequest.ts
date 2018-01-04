export class ReturnCalculationRequest {
    public SiNumber: number;
    public ReturnCalculationId: number;
    public FederationNumber: number;

    public IsFraude: boolean;
    public IsError: boolean;
    public Reason: string;
    public OtherReason: string;
    public Remark: string;
    public Insz: string;
    public CreatedBy: string;

    public ReturnByDeduction: number;
    public ReturnByOGM: number;
    
    public RegularLetterCreateDate: Date;
    public RegularLetterCreatedBy: string;
    public SignedLetterCreateDate: Date;
    public SignedLetterCreatedBy: string;

    public ObjectionMade: boolean;
    public ObjectionDate: Date;
    public ReturnStarted: boolean;

    public ReturnLines: ReturnCalculationLine[];

    constructor() {
        this.ReturnLines = new Array<ReturnCalculationLine>();
        this.IsFraude = false;
    }

    
}

export class ReturnCalculationLine {
    public Kind: ReturnCalculationKind;
    public PaymentLines: ReturnCalculationPayment[];

    constructor() {
        this.PaymentLines = new Array<ReturnCalculationPayment>();
    }
}

export class ReturnCalculationPayment {
    constructor(public Amount: number,
        public UnCode: number,
        public StartDate: Date,
        public EndDate: Date,
        public SendDate: Date) {

    }
}

export enum ReturnCalculationKind {
    ZVZ = 1,
    BOB = 2,
    THAB = 3
}

export enum ReturnCalculationType {
    NothingToReturn = 0,
    FullDeduction = 1,
    PartialDeduction = 2,
    NonDeductable = 3
}