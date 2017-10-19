import { ComponentFixture } from '@angular/core/testing';
import { SearchModel } from './../components/search/searchbyname/search';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class SearchPersonService {
    private url: string = "api/people";

    constructor(private http: Http) { }
    
    public getPeopleByName = (search: SearchModel) => {
        return this.http.get(this.url + '?firstname=' + search.firstName || "%" + '&name=' + search.name)
                        .map((resp: Response) => {
                            return resp.json();
                        });
    }
}