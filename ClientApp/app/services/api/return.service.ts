import { Injectable } from "@angular/core";
import { HttpClient, HttpRequest, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";
import { Attachment, ReturnCalculationResponse, ReturnCalculationRequest } from "../../models/index";

@Injectable()
export class ReturnsService {

    constructor(private http: HttpClient, 
        private urlService: UrlService) {}

    public getReturnCalculation(returnItem: ReturnCalculationRequest): Observable<ReturnCalculationResponse> {
        let url = this.urlService.createUrl('returncalculations');
        
        return this.http.post<ReturnCalculationResponse>(url, returnItem);
    }

}
