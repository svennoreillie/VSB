import { Validators, FormControl } from "@angular/forms";
import { Component } from "@angular/core";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { InszSearchModel } from "./../models/search";

@Component({
    selector: "search-by-insz",
    templateUrl: "search-by-insz.component.html",
})
export class SearchByInszComponent extends SelectPersonController  {
    public searchModel: InszSearchModel = new  InszSearchModel();
    public insz = new FormControl("", [Validators.required]);

    constructor(peopleService: PeopleService) {
        super(peopleService);
    }

    public search(): void {
        if (this.searchModel.insz) {
            this.doSearch(this.searchModel);
        }
    }
}
