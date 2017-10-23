import { Component, OnInit } from "@angular/core";
import { PeopleService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { NameSearchModel } from "./../models/search";

@Component({
    selector: "search-by-name",
    templateUrl: "search-by-name.component.html",
})
export class SearchByNameComponent extends SelectPersonController  {
    public searchModel: NameSearchModel = new  NameSearchModel();

    constructor(private peopleService: PeopleService) {
        super();
    }

    public search(): void {
        if (this.searchModel.name) {
            this.people = this.peopleService.search(this.searchModel);
        }
    }
}
