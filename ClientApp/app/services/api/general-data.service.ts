import { Version } from "./../../models/admin/version";
import { Environment } from "./../../models/admin/environment";
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

    public getVersion = (): Observable<Version> => {
        const url = this.url.createUrl("admin", "versions");
        return this.http.get<Version>(url);
    }

    public getEnvironment = (): Observable<Environment> => {
        const url = this.url.createUrl("admin", "environment");
        return this.http.get<Environment>(url);
    }

}
