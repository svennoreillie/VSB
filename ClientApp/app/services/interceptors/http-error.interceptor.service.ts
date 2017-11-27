import {
    TranslateService
} from "./../../directives/translate/translate.service";
import {
    Injectable,
    Injector
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
import 'rxjs/add/operator/do';
import 'rxjs/add/observable/throw';
import {
    HttpResponse
} from "@angular/common/http";
import {
    NotificationService
} from "../index";

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private injector: Injector) {}

    intercept(req: HttpRequest < any > , next: HttpHandler): Observable < HttpEvent < any >> {

        let notificationService: NotificationService = this.injector.get(NotificationService);
        let translateService: TranslateService = this.injector.get(TranslateService);


        return next.handle(req).do(event => {
                // Remember, there may be other events besides just the response.
                if (event instanceof HttpResponse) {
                    if (!event.ok) {
                        this.process(event, translateService, notificationService);
                    }
                }
            })
            .catch(err => {
                this.process(err, translateService, notificationService);
                return Observable.throw(err);
            });;

    }

    private process(event: HttpResponse<any>, translateService: TranslateService, notificationService: NotificationService) {
        let header = event.headers.get('command');
        let message: string;
        if (header != null) {
            message = translateService.getReplacementValue(`Error ${header}`);
        }
        else {
            message = translateService.getReplacementValue(`General HTTP error`);
        }
        notificationService.showError(`${message} - ${event.statusText}`);
    }
}