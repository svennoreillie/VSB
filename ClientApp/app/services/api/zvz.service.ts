import { HttpCacheService } from "./../cache/http-cache.service";
import { ZVZContract } from "./../../models/zvz/contract";
import { PersonModel } from "./../../models/person";
import { ZVZContribution, ZVZWarranty, ZVZLetter, ZVZPayment } from "./../../models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";

@Injectable()
export class ZvzService {

    constructor(private http: HttpClient, 
        private urlService: UrlService, 
        private cacheService: HttpCacheService) {}

    public getContributions(sinumber: number): Observable<ZVZContribution[]> {
        let url = this.urlService.createUrl(`zvzcontributions/${sinumber}`); 
        return this.cacheService.get<ZVZContribution[]>(url).shareReplay(1);
    }

    public getContract(sinumber: number): Observable<ZVZContract> {
        let url = this.urlService.createUrl(`zvzcontracts/${sinumber}`); 
        return this.cacheService.get<ZVZContract>(url).shareReplay(1);
    }

    public getWarranties(sinumber: number): Observable<ZVZWarranty[]>  {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}`); 
        return this.cacheService.get<ZVZWarranty[]>(url);
    }

    public getPayments(sinumber: number): Observable<ZVZPayment[]> {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}/payments`); 
        return this.cacheService.get<ZVZPayment[]>(url).shareReplay(1);
    }

    public getSpecificPayments(sinumber: number, requestDate: Date): Observable<ZVZPayment[]> {
        let url = this.urlService.createUrl(`zvzwarranties/${sinumber}/payments/${requestDate}`); 
        return this.cacheService.get<ZVZPayment[]>(url).shareReplay(1);
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
        
        return this.cacheService.get<ZVZLetter[]>(url).shareReplay(1);
    }

}

class ZvzLetterSearchModel {
    federation: number;
    membernr: number;
    sex: number;
    birthdate: Date;
    sinumber: number;
}
