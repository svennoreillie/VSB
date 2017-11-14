import { ZVZContribution, ZVZWarranty, ZVZLetter, ZVZPayment } from "./../../models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";

@Injectable()
export class ZvzService {

    constructor(private http: HttpClient, private urlService: UrlService) {}

    public getContributions(sinumber: number): Observable<ZVZContribution[]> {
        let url = this.urlService.createUrl(`zvzcontributions/${sinumber}`); 
        return this.http.get<ZVZContribution[]>(url);
    }

    public getWarranties(sinumber: number): Observable<ZVZWarranty[]>  {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}`); 
        return this.http.get<ZVZWarranty[]>(url);
    }

    public getPayments(sinumber: number): Observable<ZVZPayment[]> {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}/payments`); 
        return this.http.get<ZVZPayment[]>(url);
    }

    public getSpecificPayments(sinumber: number, requestDate: Date): Observable<ZVZPayment[]> {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}/payments/${requestDate}`); 
        return this.http.get<ZVZPayment[]>(url);
    }

    public getLetters(letterSearchModel: ZvzLetterSearchModel): Observable<ZVZLetter[]> {
        let url = this.urlService.createUrl(`zvzletters`); 
        url = this.urlService.addQueryParameters(url, letterSearchModel);
        
        return this.http.get<ZVZLetter[]>(url);
    }

}
