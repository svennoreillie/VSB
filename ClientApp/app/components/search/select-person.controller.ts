import { PeopleService } from "./../../services/api/people.service";
import { Observable } from "rxjs/Observable";
import { PersonModel } from "../../models";

export class SelectPersonController {
    public people: Observable<PersonModel[]>;
    public selectedPerson: PersonModel;

    constructor(private peopleS: PeopleService) {

    }

    public selectPerson(person: PersonModel): void  {
        this.peopleS.setActivePerson(person);
    }

    public downloadList(): void {
        console.error("TODO => download list");
    }
}
