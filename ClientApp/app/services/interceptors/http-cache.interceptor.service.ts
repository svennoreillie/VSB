import {
    HttpCacheService
} from "./../cache/http-cache.service";
import {
    Injectable, Injector
} from '@angular/core';
import {
    HttpInterceptor,
    HttpEvent,
    HttpHandler,
    HttpRequest
} from '@angular/common/http';
import {
    Observable
} from 'rxjs/Observable';
import {
    CacheService
} from "../index";
import 'rxjs/add/operator/do';
import {
    HttpResponse
} from "@angular/common/http";

@Injectable()
export class HttpCacheInterceptor implements HttpInterceptor {

    constructor(private injector: Injector) {  }

    intercept(req: HttpRequest < any > , next: HttpHandler): Observable < HttpEvent < any >> {
        let cacheService = this.injector.get(CacheService);

        //only cache gets
        if (req.method !== 'GET') {
            return next.handle(req);
        }

        //check cache
        let cachedData = cacheService.get(req.urlWithParams);
        if (cachedData) {
            //a cached response exists. Serve it instead of forwarding the request to the next handler.
            return cachedData;
        }

        //nothing in cache => create new
        let observable = next.handle(req).do(event => {
            // Remember, there may be other events besides just the response.
            if (event instanceof HttpResponse) {
                // Update the cache.
                cacheService.update(req.urlWithParams, event);
            }
        });

        cacheService.add(req.urlWithParams, observable);
        return observable;
    }
}