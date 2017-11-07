import { PeopleService } from "./../../services/api/people.service";
import { Observable } from "rxjs/Observable";
import { Person } from "../../models";

export class SelectPersonController {
    public people: Observable<Person[]>;
    public selectedPerson: Person;

    constructor(private peopleS: PeopleService) {

    }

    public selectPerson = (person: Person) => {
        this.peopleS.setActivePerson(person);
    }

    public downloadList = () => {
        console.error("TODO => download list");
    }
}
