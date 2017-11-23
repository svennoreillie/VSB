import { BOBContact } from "./../../models/bob/contact";
import { BOBPayment, BOBCertificate, BOBLetter } from "./../../models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";
import { HttpCacheService } from "../index";

@Injectable()
export class BobService {

    constructor(private http: HttpClient, 
        private urlService: UrlService, 
        private cacheService: HttpCacheService) {}

    public getBOBPerson = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber);
        return this.cacheService.get(url).share();
    }

    public getBOBAddress = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "address");
        return this.cacheService.get(url).share();
    }

    public getBOBAccount = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "acccount");
        return this.cacheService.get(url).share();
    }

    public getBOBContact = (sinumber: number): Observable <BOBContact> => {
        let url = this.createUrl(sinumber, "contact");
        return this.cacheService.get<BOBContact>(url).share();
    }

    public getCertificates = (sinumber: number): Observable<BOBCertificate[]> => {
        let url = this.createUrl(sinumber, "certificates");
        return this.cacheService.get<BOBCertificate[]>(url).share();
    }

    public getPayments = (sinumber: number): Observable<BOBPayment[]> => {
        let url = this.createUrl(sinumber, "payments");
        return this.cacheService.get<BOBPayment[]>(url).share();
    }

    public getLetters = (sinumber: number): Observable<BOBLetter[]> => {
        let url = this.createUrl(sinumber, `letters`);
        return this.cacheService.get<BOBLetter[]>(url).share();
    }

    public downloadBobForm(sinumber: number): Observable<Blob> {
        let url = this.createUrl(sinumber, `form`);
        return this.http.get(url, { responseType: "blob"});
    }



    private createUrl(sinumber: number, endpoint ? : string): string {
        if (!endpoint) return this.urlService.createUrl(`bobpeople/${sinumber}`);
        return this.urlService.createUrl(`bobpeople/${sinumber}/${endpoint}`);
    }
}
