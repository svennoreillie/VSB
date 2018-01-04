import { Payment } from "./../payment";
export interface BOBPayment extends Payment {
    certificateId: string;
    currency: string;
    accountNb: string;
    beginDate: Date;
    endDate: Date;
    sendDate: Date;
    amount: number;
    unCode: number;
}    
