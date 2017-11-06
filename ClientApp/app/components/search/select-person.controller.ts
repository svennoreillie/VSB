import { Observable } from "rxjs/Observable";
import { Person } from "../../models";

export class SelectPersonController {
    public people: Observable<Person[]>;
    public selectedPerson: Person;

    public selectPerson = (person: Person) => {
        console.error(`TODO => implement person ${person}`);
    }

    public downloadList = () => {
        console.error("TODO => donwload list");
    }
}
