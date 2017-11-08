import { Injectable } from "@angular/core";
import { Http, Response } from "@angular/http";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/operator/shareReplay";
import { Observable } from "rxjs/Observable";
import { UrlService } from "./../url/url.service";

@Injectable()
export class BobService {

    constructor(private http: Http, private urlService: UrlService) {}

    public getBOBPerson = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber);
        return this.makeCall(url);
    }

    public getBOBAddress = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "address");
        return this.makeCall(url);
    }

    public getBOBAccount = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "acccount");
        return this.makeCall(url);
    }

    public getBOBContact = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "contact");
        return this.makeCall(url);
    }

    public getCertificates = (sinumber: number): Observable < BOBCertificate[] > => {
        let url = this.createUrl(sinumber, "certificates");
        return this.makeCall(url);//.map((json:any) => json as BOBCertificate[]);
    }

    public getPayments = (sinumber: number): Observable < any > => {
        let url = this.createUrl(sinumber, "payments");
        return this.makeCall(url);
    }

    public getLetters = (sinumber: number, certificateid: string): Observable < any > => {
        let url = this.createUrl(sinumber, `/certificates/${certificateid}/letters`);
        return this.makeCall(url);
    }




    private makeCall(url: string): Observable < any > {
        return this.http.get(url)
            .map((resp: Response) => {
                return resp.json();
            })
            .shareReplay(1);
    }

    private createUrl(sinumber: number, endpoint ? : string): string {
        if (!endpoint) return this.urlService.createUrl(`bobpeople/${sinumber}`);
        return this.urlService.createUrl(`bobpeople/${sinumber}/${endpoint}`);
    }
}
