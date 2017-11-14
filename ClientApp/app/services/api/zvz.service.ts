import { PersonModel } from "./../../models/person";
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

    public getLetters(person: PersonModel): Observable<ZVZLetter[]> {
        let url = this.urlService.createUrl(`zvzletters`); 

        let model = new ZvzLetterSearchModel();
        model.birthdate = person.birthDate;
        model.federation = person.federationNumber;
        model.membernr = parseInt(person.memberNumber);
        model.sex = person.sex;
        model.sinumber = person.siNumber;

        url = this.urlService.addQueryParameters(url, model);
        
        return this.http.get<ZVZLetter[]>(url);
    }

}

class ZvzLetterSearchModel {
    federation: number;
    membernr: number;
    sex: number;
    birthdate: Date;
    sinumber: number;
}
