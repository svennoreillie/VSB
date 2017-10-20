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
        const url = this.url.createUrl("admin", "version");
        return this.http.get(url)
                 .map((response) => response.toString());
    }

}
