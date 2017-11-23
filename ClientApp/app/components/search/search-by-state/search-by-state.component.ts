import {
    Subscription
} from "rxjs/Subscription";
import {
    PersonModel
} from "./../../../models/person";
import {
    Component,
    OnInit,
    OnDestroy
} from "@angular/core";
import {
    PeopleService
} from "../../../services";
import {
    SelectPersonController
} from "../select-person.controller";
import {
    GeneralDataService
} from "./../../../services/api/general-data.service";
import {
    StateSearchModel
} from "./../models/search";

@Component({
    selector: "search-by-state",
    templateUrl: "search-by-state.component.html",
    styleUrls: ["search-by-state.component.css"]
})
export class SearchByStateComponent extends SelectPersonController implements OnInit, OnDestroy {
    
    private generalDataSub: Subscription;
    public peopleLoading: boolean;
    private allLoaded: boolean = false;;
    private subscription: Subscription;
    private loadCount: number = 20;

    public searchModel: StateSearchModel = new StateSearchModel();
    public environment: number = 300;
    public stateRejectedDate: Date;
    public stateCompletedDate: Date;
    public peopleList: PersonModel[] = [];

    constructor(private peopleServices: PeopleService,
        private generalDataService: GeneralDataService) {
        super(peopleServices);
    }

    public ngOnInit(): void {
        this.generalDataSub = this.generalDataService.getEnvironment()
            .subscribe(
                (value) => {
                    this.environment = value.environment;
                    this.searchModel.federation = value.environment;
                },
                (error) => {
                    console.log(error);
                }
            );
    }

    public ngOnDestroy(): void {
        this.generalDataSub.unsubscribe();
        if (this.subscription) this.subscription.unsubscribe();
    }

    public search(): void {
        if (!this.isSearchDisabled()) {
            this.allLoaded = false;
            this.peopleList = [];
            this.peopleLoading = true;
            this.searchModel.limit = this.loadCount;
            if (this.stateCompletedDate) this.searchModel.stateCompletedDate = this.stateCompletedDate.toISOString();
            if (this.stateRejectedDate) this.searchModel.stateRejectedDate = this.stateRejectedDate.toISOString();
            this.makeSearchCall();
        }
    }

    public downloadList() {
        this.doDownloadList(this.searchModel);
    }

    public isSearchDisabled = (): boolean => {
        if (!this.searchModel) return true;
        const sm = this.searchModel;
        if (!sm.pillar) return true;
        if (!(sm.stateCompleted || sm.stateInitiated || sm.stateRejected)) return true;
        if (sm.stateRejected && sm.stateRejectedDate == null) return true;
        if (sm.stateCompleted && sm.stateCompletedDate == null) return true;
        if (sm.federation <= 0) return true;
        return false;
    }

    public loadMore() {
        if (this.allLoaded) return;
        this.searchModel.skip += this.loadCount;
        this.makeSearchCall();
    }

    private makeSearchCall() {
        this.subscription = this.peopleServices.search(this.searchModel)
            .subscribe(data => {
                    this.peopleList = this.peopleList.concat(data);
                    if (data.length < this.loadCount) this.allLoaded = true;
                },
                (error) => console.error(error),
                () => this.peopleLoading = false);
    }
}