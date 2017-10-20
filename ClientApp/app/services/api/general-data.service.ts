import { UrlService } from './../url/url.service';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class GeneralDataService {

    constructor(private http: Http,
    private url: UrlService) { }
    

    public getVersion = () : Observable<string> => {
        let url = this.url.createUrl('admin', 'version');
        return this.http.get(url)
                 .map(response => response.toString());
    }

    
}