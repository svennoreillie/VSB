import { HttpClient } from "@angular/common/http";
import { PersonModel } from "./../../models/person";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { SearchModel } from "./../../components/search/models/search";
import { UrlService } from "./../url/url.service";
import { ReplaySubject } from "rxjs/ReplaySubject";

@Injectable()
export class PeopleService {
    private activePersonSubject: ReplaySubject<PersonModel|null>;

    constructor(private http: HttpClient, private urlService: UrlService) { 
        this.activePersonSubject = new ReplaySubject<PersonModel|null>();
    }

    public clearActivePerson() {
        this.activePersonSubject.next(null);
    }

    public setActivePerson(p : PersonModel) {
        if (p === null || p === undefined) this.clearActivePerson();
        else this.activePersonSubject.next(p);
    }
    
    public get activePerson(): Observable<PersonModel|null> {
        return this.activePersonSubject.asObservable();
    }

    public search = (search: SearchModel): Observable<PersonModel[]> => {
        let url = this.urlService.createUrl("people");
        url = this.urlService.addQueryParameters(url, search);

        return this.http.get<PersonModel[]>(url);
    }

    public get(sinumber: number): Observable<PersonModel> {
        let url = this.urlService.createUrl("people", sinumber.toString());
        return this.http.get<PersonModel>(url);
    }
}
