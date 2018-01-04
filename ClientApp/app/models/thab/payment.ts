import { Payment } from "../index";

export interface THABPayment extends Payment {
    Amount: number;
    Currency: string;
    SendDate: Date;
    Iban: string;
    PeriodStart: Date;
    PeriodEnd: Date;
    UnCode: number;
}   
    