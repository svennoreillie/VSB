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

    public getAttachments(sinumber: number, noCache?: boolean): Observable<Attachment[]> {
        let url = this.urlService.createUrl('attachments', sinumber.toString());
        if (noCache) url = this.urlService.disableCache(url);
        return this.http.get<Attachment[]>(url);
    }


    public postAttachment(sinumber: number, formData: any): Observable<Attachment> {
        let url = this.urlService.createUrl('attachments', sinumber.toString());
        return this.http.post<Attachment>(url, formData);
    }

    public removeAttachment(sinumber: number, name: string): Observable<Attachment> {
        let url = this.urlService.createUrl('attachments', sinumber.toString(), name);
        return this.http.delete < Attachment>(url);
    }

    public downloadAttachment(sinumber: number, name: string): Observable<Blob> {
        let url = this.urlService.createUrl('attachments', sinumber.toString(), name);
        return this.http.get(url, { responseType: "blob"});
    }
}
