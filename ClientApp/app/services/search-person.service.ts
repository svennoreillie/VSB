import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class SearchPersonService {
    private url: string = "api/people";

    constructor(private http: Http) { }
    
    public getPeopleByName = (firstName: string, name: string) => {
        return this.http.get(this.url + '?firstname=' + firstName + '&name=' + name)
                        .map((resp: Response) => {
                            return resp.json();
                        });
    }
}