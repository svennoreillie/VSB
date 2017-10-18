import { Person } from '../../models/person';
import { Observable } from 'rxjs/Observable';

export class SelectPersonController {
    public people: Observable<Person[]>;
    public selectedPerson: Person;

    public selectPerson = (person: Person) => {
        console.error("TODO");
    }

    public downloadList = () => {
        console.error("TODO");
    }
}