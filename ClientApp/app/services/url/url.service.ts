import { Injectable } from "@angular/core";
import { ConfigService } from "./../config/config";

@Injectable()
export class UrlService {

    constructor(private config: ConfigService) { }

    public createUrl(...resources: string[]): string {
        let url: string = this.config.config.api.apiUrl;

        resources.forEach((resource) => {
            if (!url.endsWith("/")) url += "/";
            url += resource;
        });

        return url;
    }

    public addQueryParameters(url: string, queryParameters: any): string {
        if (queryParameters !== null) url += "?";
        const qpString: string = this.encodeQueryData(queryParameters);
        if (qpString.length > 0) url += qpString;
        return url;
    }

    public disableCache(url: string): string {
        let random: string = Math.random().toString(36).replace(/[^a-z]+/g, '').substr(0, 5);
        return this.addQueryParameters(url, { nocache: random });
    }

    private encodeQueryData = (data: any): string => {
        const ret = [];
        for (const key in data) {
            if (data.hasOwnProperty(key)) {
                const element = data[key];
                if (element) {
                    ret.push(encodeURIComponent(key) + "=" + encodeURIComponent(element));
                }
            }
        }
        return ret.join("&");
     }
}
