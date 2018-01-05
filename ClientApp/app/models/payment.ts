export interface Payment {
    accountNb: string;
    currency: string;
    beginDate: Date;
    endDate: Date;
    sendDate: Date;
    amount: number;
    unCode: number;
}