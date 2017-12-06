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
    ActiveContentPageService
} from "../index";
import 'rxjs/add/operator/do';
import {
    HttpResponse
} from "@angular/common/http";

@Injectable()
export class HttpHoldOffInterceptor implements HttpInterceptor {

    private pausedHandlers: HttpHandler[] = [];

    constructor(private injector: Injector) {  }

    intercept(req: HttpRequest < any > , next: HttpHandler): Observable < HttpEvent < any >> {
        let activeContentPageService = this.injector.get(ActiveContentPageService);

        //only hold off gets
        if (req.method !== 'GET') {
            return next.handle(req);
        }

        if (req.headers.has('vsbOrigin')) {
            var origin = req.headers.get('vsbOrigin');
            if (origin !== activeContentPageService.activePage.toString()) {
                //this is to make it look more responsive => browser has a limited amount of concurrent requests
                //we only push the ones from the active content page out immediately and delay the other ones with a second
                return next.handle(req).debounceTime(10000);
            }
        } else {
            return next.handle(req);
        }
    }
}
