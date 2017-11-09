import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/operator/shareReplay";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";

@Injectable()
export class GeneralDataService {

    constructor(private http: HttpClient,
                private url: UrlService) { }

    public getVersion = (): Observable<string> => {
        const url = this.url.createUrl("admin", "versions");
        return this.http.get<string>(url);
    }

    public getEnvironment = (): Observable<number> => {
        const url = this.url.createUrl("admin", "environment");
        return this.http.get<number>(url);
    }

}
