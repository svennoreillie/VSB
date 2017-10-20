import { Injectable } from "@angular/core";
import { ConfigService } from "./../config/config";

@Injectable()
export class UrlService {

    constructor(private config: ConfigService) { }

    public createUrl(...resources: string[]): string {
        let url: string = this.config.config.api.apiUrl;
        resources.forEach((resource) => {
            url += (resource + "/");
        });
        return url;
    }

    public addQueryParameters(url: string, queryParameters: any): string {
        if (queryParameters !== null) url += "?";
        const qpString: string = this.encodeQueryData(queryParameters);
        if (qpString.length > 0) url += qpString;
        return url;
    }

    private encodeQueryData = (data: any): string => {
        const ret = [];
        for (const d of data) {
            ret.push(encodeURIComponent(d) + "=" + encodeURIComponent(data[d]));
        }
        return ret.join("&");
     }
}
