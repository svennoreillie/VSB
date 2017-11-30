import { Injectable } from "@angular/core";
import { HttpClient, HttpRequest, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";
import { Attachment } from "../../models/index";

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

    public getAttachments(sinumber: number) : Observable<Attachment[]> {
        let url = this.urlService.createUrl('attachments', sinumber.toString());
        return this.http.get<Attachment[]>(url);
    }

    public postAttachment(sinumber: number, username: string, formData: any): Observable<Attachment> {
        let url = this.urlService.createUrl('attachments', sinumber.toString());
        url = this.urlService.addQueryParameters(url, { username: username });

        // let request = new HttpRequest("POST", url, formData) ;
        // request.headers.delete('Content-Type');

        return this.http.post<Attachment>(url, formData);
      }
}
