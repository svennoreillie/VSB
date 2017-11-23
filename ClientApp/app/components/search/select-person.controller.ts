import { OnDestroy } from "@angular/core";
import {
    PeopleService
} from "./../../services/api/people.service";
import {
    Observable
} from "rxjs/Observable";
import {
    PersonModel
} from "../../models";
import {
    SearchModel
} from "./models/search";
import { Subscription } from "rxjs/Subscription";

export class SelectPersonController implements OnDestroy{
    
    private downloadBob: Subscription;
    public people: Observable < PersonModel[] > ;
    public selectedPerson: PersonModel;

    constructor(private peopleService: PeopleService) {

    }

    public ngOnDestroy(): void {
        if (this.downloadBob) this.downloadBob.unsubscribe();
    }

    public doSearch(searchModel: SearchModel) {
        this.people = this.peopleService.search(searchModel).share();
    }

    public selectPerson(person: PersonModel): void {
        this.peopleService.setActivePerson(person);
    }

    public doDownloadList(searchModel: SearchModel): void {
            this.downloadBob = this.peopleService.downloadCSV(searchModel)
                .subscribe(blob => {
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = `Opvraging lijst personen VSB.csv`;
                    link.click();
                }, error => console.error(error));
    }
}