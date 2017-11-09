export interface BOBPayment {
    certificateId: string;
    beginDate: Date;
    endDate: Date;
    sendDate: Date;
    amount: number;
    currency: string;
    accountNb: string;
    unCode: number;
}    
