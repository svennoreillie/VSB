import { Component } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { MemberNrSearchModel } from "./../models/search";

@Component({
    selector: "search-by-membernr",
    templateUrl: "search-by-membernr.component.html",
})
export class SearchByMembernrComponent extends SelectPersonController  {
    public searchModel: MemberNrSearchModel = new  MemberNrSearchModel();
    public federation = new FormControl("", [Validators.required]);
    public memberNr = new FormControl("", [Validators.required]);

    constructor(peopleService: PeopleService) {
        super(peopleService);
    }

    public search(): void {
        if (this.isInvalid()) return;
        this.doSearch(this.searchModel);
    }

    public isInvalid(): boolean {
         return this.federation.invalid || this.memberNr.invalid;
    }

    public downloadList() {
        this.doDownloadList(this.searchModel);
    }
}
