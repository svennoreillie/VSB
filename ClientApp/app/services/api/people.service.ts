import { Subscription } from "rxjs/Subscription";
import { HttpClient } from "@angular/common/http";
import { PersonModel } from "./../../models/person";
import { Injectable, OnDestroy } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { SearchModel } from "./../../components/search/models/search";
import { UrlService } from "./../url/url.service";
import { ReplaySubject } from "rxjs/ReplaySubject";

@Injectable()
export class PeopleService implements OnDestroy {
    
    private activePersonSubject: ReplaySubject<PersonModel|null>;
    private activePersonFullDetailSubject: ReplaySubject<PersonModel|null>;
    private currentActivePerson: PersonModel | null;
    private updatePersonSubscription: Subscription;

    constructor(private http: HttpClient, 
        private urlService: UrlService) { 
        this.activePersonSubject = new ReplaySubject<PersonModel|null>();
        this.activePersonFullDetailSubject = new ReplaySubject<PersonModel|null>();
    }

    public ngOnDestroy(): void {
        if (this.updatePersonSubscription) this.updatePersonSubscription.unsubscribe();
    }



    public clearActivePerson() {
        this.currentActivePerson = null;
        this.activePersonSubject.next(this.currentActivePerson);
        this.activePersonFullDetailSubject.next(this.currentActivePerson);
    }

    public setActivePerson(p : PersonModel) {
        if (p === null || p === undefined) this.clearActivePerson();
        else {
            this.currentActivePerson = p;
            this.activePersonSubject.next(this.currentActivePerson);
            // get more details
            this.updatePersonSubscription = this.get(this.currentActivePerson.siNumber)
                                                .share()
                                                .subscribe(data => this.updateActivePersonWithFullDetail(data));
        }
    }

    private updateActivePersonWithFullDetail(p : PersonModel) {
        if (!p) return;
        if (p == null) return;
        if (this.currentActivePerson !== null) {
            if (this.currentActivePerson.siNumber !== p.siNumber) return;
        }
        this.currentActivePerson = p;
        this.activePersonFullDetailSubject.next(this.currentActivePerson);
    }
    
    public get activePerson(): Observable<PersonModel|null> {
        return this.activePersonSubject.asObservable();
    }

    public get activePersonFullDetails(): Observable<PersonModel|null> {
        return this.activePersonFullDetailSubject.asObservable();
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

    public downloadCSV(search: SearchModel): Observable<Blob> {
        let url = this.urlService.createUrl("people");
        search.csv = true;
        url = this.urlService.addQueryParameters(url, search);
        
        return this.http.get(url, { responseType: "blob"});
    }
}
