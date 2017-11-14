import { PersonModel } from "./../../models/person";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { PeopleService } from "./../../services/api/people.service";
import { Component, OnInit, OnDestroy } from "@angular/core";

@Component({
    selector: 'content-zvz',
    templateUrl: 'content-zvz.component.html'
})
export class ContentZvzComponent implements OnInit, OnDestroy {

    //Properties
    public activePersonSubscription: Subscription;

    public person: PersonModel | null;



    //Lifecycle hooks
    constructor(private zvzService: ZvzService, private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personChanged(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

   
    //Private Methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
        this.loadData();
    }

    private loadData() {
        if (this.person == null) return;

    }

}
