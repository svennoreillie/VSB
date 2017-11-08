import { Person } from "./../../models/person";
import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/operator/shareReplay";
import { Observable } from "rxjs/Observable";
import { SearchModel } from "./../../components/search/models/search";
import { UrlService } from "./../url/url.service";
import { ReplaySubject } from "rxjs/ReplaySubject";

@Injectable()
export class PeopleService {
    private activePersonSubject: ReplaySubject<Person|null>;

    constructor(private http: Http, private urlService: UrlService) { 
        this.activePersonSubject = new ReplaySubject<Person|null>();
    }

    public clearActivePerson() {
        this.activePersonSubject.next(null);
    }

    public setActivePerson(p : Person) {
        if (p === null || p === undefined) this.clearActivePerson();
        else this.activePersonSubject.next(p);
    }
    
    public get activePerson(): Observable<Person|null> {
        return this.activePersonSubject.asObservable();
    }

    public search = (search: SearchModel): Observable<any> => {
        let url = this.urlService.createUrl("people");
        url = this.urlService.addQueryParameters(url, search);

        return this.http.get(url)
                        .map((resp: Response) => {
                            return resp.json();
                        })
                        .shareReplay(1);
    }
}
