import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { GeneralDataService } from "./../../../services/api/general-data.service";
import { SearchModel } from "./../models/search";
import { StateSearchModel } from "./../models/search";

@Component({
    selector: "search-by-state",
    templateUrl: "search-by-state.component.html",
    styleUrls: ["search-by-state.component.css"]
})
export class SearchByStateComponent extends SelectPersonController implements OnInit {

    public searchModel: StateSearchModel = new  StateSearchModel();
    public environment: number = 300;

    constructor(private peopleService: PeopleService,
                private generalDataService: GeneralDataService) {
        super();
    }

    public ngOnInit(): void {
        this.generalDataService.getEnvironment()
                               .subscribe(
                                    (value) => { this.environment = value; },
                                    (error) => { console .log(error); }
                                );
    }

    public search(): void {
        if (!this.isSearchDisabled()) {
            this.people = this.peopleService.search(this.searchModel);
        }
    }

    public isSearchDisabled = (): boolean => {
        if (!this.searchModel) return true;
        const sm = this.searchModel;
        if (!(sm.ZVZ || sm.BOB || sm.THAB)) return true;
        if (!(sm.StateCompleted || sm.StateInitiated || sm.StateRejected)) return true;
        if (sm.StateRejected && sm.StateRejectedDate == null) return true;
        if (sm.StateCompleted && sm.StateCompletedDate == null) return true;
        if (sm.federation <= 0) return true;
        return false;
    }

    public getConfirmedMonth = (): Date => {
        return this.searchModel.StateCompletedDate;
    }

    public updateConfirmedMonth(newValue: Date): void {
        this.searchModel.StateCompletedDate = newValue;
    }

    public getDecisionMonth(): Date {
        return this.searchModel.StateRejectedDate;
    }

    public updateDecisionMonth(newValue: Date): void {
        this.searchModel.StateRejectedDate = newValue;
    }
}
