import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";

@Injectable()
export class GeneralDataService {

    constructor(private http: Http,
                private url: UrlService) { }

    public getVersion = (): Observable<string> => {
        const url = this.url.createUrl("admin", "versions");
        return this.http.get(url)
                 .map((response) => {
                     return response.text();
                    });
    }

    public getEnvironment = (): Observable<number> => {
        const url = this.url.createUrl("admin", "environment");
        return this.http.get(url)
                 .map((response) => {
                     return Number(response.text());
                    });
    }

}
