import { Component, OnInit } from '@angular/core';
import { SelectPersonController } from '../select-person.controller';
import { SearchPersonService } from '../../../services/search-person.service';

@Component({
    selector: 'search-by-name',
    templateUrl: 'search-by-name.component.html'
})

export class SearchByNameComponent extends SelectPersonController implements OnInit {
    public firstName: string;
    public surName: string;

    constructor(private searchPersonService: SearchPersonService) { 
        super();
    }

    ngOnInit() { }

    public search(): void {
        if (!this.firstName) this.firstName = "%";
        if (this.firstName && this.surName) {
            this.people = this.searchPersonService.getPeopleByName(this.firstName, this.surName);
        }
    }
}