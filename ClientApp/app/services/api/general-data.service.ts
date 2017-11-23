import { Version } from "./../../models/admin/version";
import { Environment } from "./../../models/admin/environment";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";
import { HttpCacheService } from "../index";

@Injectable()
export class GeneralDataService {

    constructor(private http: HttpClient,
                private url: UrlService, 
                private cacheService: HttpCacheService) { }

    public getVersion = (): Observable<Version> => {
        const url = this.url.createUrl("admin", "versions");
        return this.cacheService.get<Version>(url);
    }

    public getEnvironment = (): Observable<Environment> => {
        const url = this.url.createUrl("admin", "environment");
        return this.cacheService.get<Environment>(url);
    }

    public getUser = (): Observable<any> => {
        const url = this.url.createUrl("admin", "users");
        return this.cacheService.get(url);
    }

}
