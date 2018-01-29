import { UrlService } from "./../url/url.service";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { THABCertificate, THABPayableAmount, THABPayment } from "../../models";
import { Observable } from "rxjs/Observable";
import { THABNotification } from "../../models/thab/notification";
import { THABLetter } from "../../models/thab/letter";

@Injectable()
export class ThabService {

     constructor(private http: HttpClient, 
        private urlService: UrlService) {}

    public getCertificates(sinumber: number, insz: string): Observable<THABCertificate[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { insz: insz});
        
        return this.http.get<THABCertificate[]>(url).share();
    }

    public getRemark(sinumber: number, certificate: THABCertificate): Observable<string> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'remarks', certificate.referenceDate.toString());
        
        return this.http.get(url, { responseType: "text"});
    }

    public saveRemark(sinumber: number, certificate: THABCertificate): Observable<any> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'remarks');
        
        return this.http.post(url, { remark: certificate.remark, referenceDate: certificate.referenceDate });
    }

    public getPayableAmounts(sinumber: number, certificateid: string): Observable<THABPayableAmount[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'payableamounts', certificateid);
        
        return this.http.get<THABPayableAmount[]>(url).share();
    }

    public getPayments(sinumber: number, insz: string): Observable<THABPayment[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'payments');
        url = this.urlService.addQueryParameters(url, { insz: insz});
        
        return this.http.get<THABPayment[]>(url).share();
    }

    public getFODPayments(sinumber: number, insz: string): Observable<THABPayment[]> {
        let url = this.urlService.createUrl('thabcertificates', sinumber.toString(), 'fodpayments');
        url = this.urlService.addQueryParameters(url, { insz: insz });

        return this.http.get<THABPayment[]>(url).share();
    }

    public getLetters = (sinumber: number, insz: string): Observable<THABLetter[]> => {
        let url = this.urlService.createUrl('thabletters', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { insz: insz});
        return this.http.get<THABLetter[]>(url).share();
    }

    public getNotifications(certificateid: string): Observable<THABNotification> {
        let url = this.urlService.createUrl('thabnotifications', certificateid);
        
        return this.http.get<THABNotification>(url);
    }

    public downloadForm(sinumber: number, referenceDate: Date): Observable<Blob> {
        let url = this.urlService.createUrl('thabforms', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { referenceDate: referenceDate });
        return this.http.get(url, { responseType: "blob"});
    }

    public downloadCalculation(sinumber: number, certificateId: string, insz: string): Observable<Blob> {
        let url = this.urlService.createUrl('thabcalculationdocuments', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { certificateId: certificateId, insz: insz });
        return this.http.get(url, { responseType: "blob"});
    }
}
