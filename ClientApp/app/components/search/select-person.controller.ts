import { PeopleService } from "./../../services/api/people.service";
import { Observable } from "rxjs/Observable";
import { PersonModel } from "../../models";

export class SelectPersonController {
    public people: Observable<PersonModel[]>;
    public selectedPerson: PersonModel;

    constructor(private peopleS: PeopleService) {

    }

    public selectPerson = (person: PersonModel) => {
        this.peopleS.setActivePerson(person);
    }

    public downloadList = () => {
        console.error("TODO => download list");
    }
}
