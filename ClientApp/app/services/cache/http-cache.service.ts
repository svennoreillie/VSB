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
import {
    Http,
    Response
} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/interval';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class HttpCacheService implements OnDestroy {

    private cacheTimeMinutes: number = 5;
    private subscription: Subscription;
    private items: CacheItem < any > [] = [];

    constructor(private http: Http, @Inject(PLATFORM_ID) private platformId: Object) { }

    public get < T > (url: string): Observable < T > {
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
        let observable = this.http.get(url).map((response: Response) => {
                let item = this.items.find(x => x.url === response.url);
                if (item != undefined) {
                    item.loading = false;
                    item.observable = null;
                }

                if (response.status == 400) {
                    //do something
                } else if (response.status == 200) {
                    let data = response.json();
                    if (item != undefined) item.data = data;
                    return data;
                }

                return undefined;
            })
            .share() as Observable < T > ;
        let timestamp =  (new Date()).getTime();
        timestamp += this.cacheTimeMinutes * 60 * 1000;
        let newItem = new CacheItem(url, true, timestamp, observable);

        this.items.push(newItem);

        return observable;
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
        public loading: boolean,
        public deleteTime: number,
        public observable: Observable < T > | null) {

    }
    public data: T;
}