import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import { Observable } from "rxjs/Observable";
import { SearchModel } from "./../../components/search/models/search";
import { UrlService } from "./../url/url.service";

@Injectable()
export class PeopleService {

    constructor(private http: Http, private urlService: UrlService) { }

    public search = (search: SearchModel): Observable<any> => {
        let url = this.urlService.createUrl("people");
        url = this.urlService.addQueryParameters(url, search);

        return this.http.get(url)
                        .map((resp: Response) => {
                            return resp.json();
                        });
    }
}
