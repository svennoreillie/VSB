import { Payment } from "./../payment";
export interface BOBPayment extends Payment {
    certificateId: string;
}    
