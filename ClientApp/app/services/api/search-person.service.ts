import { Injectable } from "@angular/core";
import { ComponentFixture } from "@angular/core/testing";
import { Http, Response } from "@angular/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import { Observable } from "rxjs/Observable";
import { SearchModel } from "./../../components/search/searchbyname/search";

@Injectable()
export class SearchPersonService {
    private url: string = "api/people";

    constructor(private http: Http) { }

    public getPeopleByName = (search: SearchModel): Observable<any> => {
        return this.http.get(this.url + "?firstname=" + search.firstName || "%" + "&name=" + search.name)
                        .map((resp: Response) => {
                            return resp.json();
                        });
    }
}
