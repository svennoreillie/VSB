import { Injectable } from "@angular/core";

@Injectable()
export class ThabService {

    // constructor(private http: HttpClient, private urlService: UrlService) {}

    // public getCertificates = (sinumber: number): Observable<BOBCertificate[]> => {
    //     let url = this.createUrl(sinumber, "certificates");
    //     return this.http.get<BOBCertificate[]>(url);
    // }

    // public getPayments = (sinumber: number): Observable<BOBPayment[]> => {
    //     let url = this.createUrl(sinumber, "payments");
    //     return this.http.get<BOBPayment[]>(url);
    // }

    // public getLetters = (sinumber: number): Observable<BOBLetter[]> => {
    //     let url = this.createUrl(sinumber, `letters`);
    //     return this.http.get<BOBLetter[]>(url);
    // }


    // private createUrl(sinumber: number, endpoint ? : string): string {
    //     if (!endpoint) return this.urlService.createUrl(`bobpeople/${sinumber}`);
    //     return this.urlService.createUrl(`bobpeople/${sinumber}/${endpoint}`);
    // }
}
