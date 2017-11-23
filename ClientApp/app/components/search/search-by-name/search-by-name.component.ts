import { Component } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { NameSearchModel } from "./../models/search";

@Component({
    selector: "search-by-name",
    templateUrl: "search-by-name.component.html",
})
export class SearchByNameComponent extends SelectPersonController  {
    public searchModel: NameSearchModel = new  NameSearchModel();

    public name = new FormControl("", [Validators.required]);

    constructor(peopleService: PeopleService) {
        super(peopleService);
    }

    public search() {
        this.doSearch(this.searchModel);
    }

    public downloadList() {
        this.doDownloadList(this.searchModel);
    }
}
