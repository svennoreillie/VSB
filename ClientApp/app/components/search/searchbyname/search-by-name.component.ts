import { Component, OnInit } from "@angular/core";
import { SearchPersonService } from "../../../services";
import { SelectPersonController } from "../select-person.controller";
import { SearchModel } from "./search";

@Component({
    selector: "search-by-name",
    templateUrl: "search-by-name.component.html",
})
export class SearchByNameComponent extends SelectPersonController  {
    public searchModel: SearchModel = new SearchModel();

    constructor(private searchPersonService: SearchPersonService) {
        super();
    }

    public search(): void {
        if (this.searchModel.name) {
            this.people = this.searchPersonService.getPeopleByName(this.searchModel);
        }
    }
}
