import { ThabService } from "./../../services/api/thab.service";
import { PersonModel } from "./../../models/person";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { PeopleService } from "./../../services/api/people.service";
import { Component, OnInit, OnDestroy } from "@angular/core";

@Component({
    selector: 'content-thab',
    templateUrl: 'content-thab.component.html'
})
export class ContentThabComponent implements OnInit, OnDestroy {

    //Properties
    public activePersonSubscription: Subscription;
    public downloadBob: Subscription;

    public person: PersonModel | null;

    public letters: Observable<any[]>;


    //Lifecycle hooks
    constructor(private thabService: ThabService, private peopleService: PeopleService) {}

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

        console.log(this.thabService);
    }

}
