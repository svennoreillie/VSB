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
    private peopleLoading: boolean;
    private allLoaded: boolean = false;;
    private subscription: Subscription;
    private loadCount: number = 15;

    public searchModel: StateSearchModel = new StateSearchModel();
    public environment: number = 300;
    public StateRejectedDate: Date;
    public StateCompletedDate: Date;
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
                    this.searchModel.Federation = value.environment;
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
            if (this.StateCompletedDate) this.searchModel.StateCompletedDate = this.StateCompletedDate.toISOString();
            if (this.StateRejectedDate) this.searchModel.StateRejectedDate = this.StateRejectedDate.toISOString();
            this.makeSearchCall();
        }
    }

    public isSearchDisabled = (): boolean => {
        if (!this.searchModel) return true;
        const sm = this.searchModel;
        if (!sm.Pillar) return true;
        if (!(sm.StateCompleted || sm.StateInitiated || sm.StateRejected)) return true;
        if (sm.StateRejected && sm.StateRejectedDate == null) return true;
        if (sm.StateCompleted && sm.StateCompletedDate == null) return true;
        if (sm.Federation <= 0) return true;
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