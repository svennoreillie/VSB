import { Component, OnInit } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { MemberNrSearchModel } from "./../models/search";
import { SearchModel } from "./../models/search";
import { InszSearchModel } from "./../models/search";

@Component({
    selector: "search-by-membernr",
    templateUrl: "search-by-membernr.component.html",
})
export class SearchByMembernrComponent extends SelectPersonController  {
    public searchModel: MemberNrSearchModel = new  MemberNrSearchModel();
    public federation = new FormControl("", [Validators.required]);
    public memberNr = new FormControl("", [Validators.required]);

    constructor(private peopleService: PeopleService) {
        super();
    }

    public search(): void {
        this.people = this.peopleService.search(this.searchModel);
    }

    public isInvalid(): boolean {
         return this.federation.invalid || this.memberNr.invalid;
    }
}
