import { Validators, FormControl } from "@angular/forms";
import { Component, OnInit } from "@angular/core";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { SearchModel } from "./../models/search";
import { InszSearchModel } from "./../models/search";

@Component({
    selector: "search-by-insz",
    templateUrl: "search-by-insz.component.html",
})
export class SearchByInszComponent extends SelectPersonController  {
    public searchModel: InszSearchModel = new  InszSearchModel();
    public insz = new FormControl("", [Validators.required]);

    constructor(private peopleService: PeopleService) {
        super();
    }

    public search(): void {
        if (this.searchModel.insz) {
            this.people = this.peopleService.search(this.searchModel);
        }
    }
}
