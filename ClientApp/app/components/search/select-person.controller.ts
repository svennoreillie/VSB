import { PeopleService } from "./../../services/api/people.service";
import { Observable } from "rxjs/Observable";
import { PersonModel } from "../../models";
import { SearchModel } from "./models/search";

export class SelectPersonController {
    public people: Observable<PersonModel[]>;
    public selectedPerson: PersonModel;

    constructor(private peopleService: PeopleService) {

    }

    public doSearch(searchModel: SearchModel) {
        this.people = this.peopleService.search(searchModel).share();
    }

    public selectPerson(person: PersonModel): void  {
        this.peopleService.setActivePerson(person);
    }

    public downloadList(): void {
        console.error("TODO => download list");
    }
}
