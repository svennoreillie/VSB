import { Component, OnInit } from "@angular/core";
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

    constructor(private peopleService: PeopleService) {
        super();
    }

    public search(): void {
        if (this.searchModel.memberNr) {
            this.people = this.peopleService.search(this.searchModel);
        }
    }
}
