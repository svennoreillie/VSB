import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";
import { HttpCacheService } from "../index";

@Injectable()
export class VSBService {

    constructor(private http: HttpClient, 
        private urlService: UrlService) {}

    public getRemark(sinumber: number): Observable<string> {
        let url = this.urlService.createUrl('vsbremarks', sinumber.toString());
        
        return this.http.get(url, { responseType: "text"});
    }

    public saveRemark(sinumber: number, remark: string): Observable<any> {
        let url = this.urlService.createUrl('vsbremarks', sinumber.toString());
        
        return this.http.post(url, { remark: remark });
    }
}
