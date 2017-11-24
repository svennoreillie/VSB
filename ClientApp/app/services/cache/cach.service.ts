import { TranslateService } from "./../../directives/translate/translate.service";
import { ErrorMessageService } from "./../error/error-message.service";
import {
    HttpClient,
    HttpResponse,
    HttpErrorResponse,
    HttpEvent
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
export class CacheService implements OnDestroy {

    private subscription: Subscription;
    private cacheTimeMinutes: number = 5;
    private items: CacheItem[] = [];

    constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

    public get(url: string): Observable<HttpEvent<any>> | null {
        let item = this.items.find(x => x.url === url);
        if (item != undefined) {
            if (item.data) return Observable.of(item.data);
            else return item.observable
        }
        return null;
    }

    public add(url: string, observable: Observable<HttpEvent<any>>) {
        this.init();

        let timestamp = (new Date()).getTime();
        timestamp += this.cacheTimeMinutes * 60 * 1000;

        let item = new CacheItem(url, timestamp, observable);
        this.items.push(item);
    }

    public update(url: string, data: HttpResponse<any>) {
        let item = this.items.find(x => x.url === url);
        if (item != undefined) {
            item.observable = null;
            item.data = data;
        }
        return null;
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
                item.observable = null;
                var index = this.items.indexOf(item, 0);
                if (index > -1) {
                    this.items.splice(index, 1);
                }
            }
        });
    }
}

class CacheItem {
    constructor(public url: string,
        public deleteTime: number,
        public observable: Observable<HttpEvent<any>>|null) {

    }
    public data: HttpResponse<any>|null;
}