import { Payment } from "./../payment";
export interface ZVZPayment extends Payment {
    currency: string;
    accountNb: string;
    beginDate: Date;
    endDate: Date;
    sendDate: Date;
    amount: number;
    unCode: number;
}    
