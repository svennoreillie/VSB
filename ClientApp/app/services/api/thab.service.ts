import { UrlService } from "./../url/url.service";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { THABCertificate, THABPayableAmount, THABPayment } from "../../models";
import { Observable } from "rxjs/Observable";

@Injectable()
export class ThabService {

     constructor(private http: HttpClient, private urlService: UrlService) {}

    public getCertificates(sinumber: number, insz: string): Observable<THABCertificate[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { insz: insz});
        
        return this.http.get<THABCertificate[]>(url);
    }

    public getRemark(sinumber: number, certificate: THABCertificate): Observable<string> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'remarks', certificate.referenceDate.toString());
        
        return this.http.get(url, { responseType: "text"});
    }

    public getPayableAmounts(sinumber: number, certificateid: string): Observable<THABPayableAmount[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'payableamounts', certificateid);
        
        return this.http.get<THABPayableAmount[]>(url);
    }

    public getPayments(sinumber: number, insz: string): Observable<THABPayment[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'payments');
        url = this.urlService.addQueryParameters(url, { insz: insz});
        
        return this.http.get<THABPayment[]>(url);
    }
}
