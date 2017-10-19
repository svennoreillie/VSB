import { SearchModel } from './search';
import { Component, OnInit } from '@angular/core';
import { SelectPersonController } from '../select-person.controller';
import { SearchPersonService } from '../../../services/search-person.service';

@Component({
    selector: 'search-by-name',
    templateUrl: 'search-by-name.component.html'
})
export class SearchByNameComponent extends SelectPersonController implements OnInit {
    public searchModel: SearchModel = new SearchModel();

    constructor(private searchPersonService: SearchPersonService) { 
        super();
    }

    ngOnInit() { }

    public search(): void {
        if (this.searchModel.name) {
            this.people = this.searchPersonService.getPeopleByName(this.searchModel);
        }
    }
}