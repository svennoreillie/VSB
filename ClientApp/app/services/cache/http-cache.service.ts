import { TranslateService } from "./../../directives/translate/translate.service";
import { ErrorMessageService } from "./../error/error-message.service";
import {
    HttpClient,
    HttpResponse,
    HttpErrorResponse
} from "@angular/common/http";
import {
    Subscription
} from "rxjs/Subscription";
import {
    Observable
} from "rxjs/Observable";
import {
    Injectable,
    OnDestroy,
    Inject,
    PLATFORM_ID
} from '@angular/core';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/retry';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/interval';
import {
    isPlatformBrowser
} from '@angular/common';

@Injectable()
export class HttpCacheService implements OnDestroy {

    private cacheTimeMinutes: number = 5;
    private subscription: Subscription;
    private items: CacheItem < any > [] = [];

    constructor(private http: HttpClient, 
        private errorService: ErrorMessageService,
        private translate: TranslateService,
        @Inject(PLATFORM_ID) private platformId: Object) {}

    public get < T > (url: string, errormessage?: string): Observable<T> {
        this.init();

        let item = this.items.find(x => x.url === url);
        if (item !== undefined) {
            if (item.data != undefined) {
                //return data
                return Observable.of(item.data);
            } else if (item.loading && item.observable != null && item.observable != undefined) {
                //return existing observable
                return item.observable;
            }
        }
        //make call 
        let observable = this.http.get < T > (url, {
                observe: 'response'
            })
            .retry(2)
            .share()
            .map(
                //Success
                (response: HttpResponse<T>) => {
                    let item = this.items.find(x => x.url === response.url);
                    if (item != undefined) {
                        item.loading = false;
                        item.observable = null;
                        item.data = response.body;
                    }

                    if (!response.ok) {
                        if (item != undefined && item.errortext) {
                            let translation = this.translate.getReplacementValue(`${item.errortext} Error`);
                            this.errorService.showError(`${translation} (${response.status})`);
                        }
                        else this.errorService.showError(`${response.status} - ${response.statusText}`);
                    }

                    if (response.body != null) return response.body;
                }
            );

        let timestamp = (new Date()).getTime();
        timestamp += this.cacheTimeMinutes * 60 * 1000;
        let newItem = new CacheItem(url, errormessage, true, timestamp, observable);

        this.items.push(newItem);

        return observable as Observable<T>;
    }
    public init(): void {
        if (isPlatformBrowser(this.platformId)) {
            if (!this.subscription) this.subscription = Observable.interval(20000).subscribe(x => this.clearCacheRoutine());
        }
    }
    public ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    private clearCacheRoutine() {
        const deleteTime: number = (new Date()).getTime();
        this.items.forEach(item => {
            if (item.deleteTime < deleteTime) {
                var index = this.items.indexOf(item, 0);
                if (index > -1) {
                    this.items.splice(index, 1);
                }
            }
        });
    }
}

class CacheItem < T > {
    constructor(public url: string,
        public errortext: string | undefined,
        public loading: boolean,
        public deleteTime: number,
        public observable: Observable<T> | null) {

    }
    public data: T;
}