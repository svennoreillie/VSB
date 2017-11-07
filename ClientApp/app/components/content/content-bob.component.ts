import { PeopleService } from "./../../services/api/people.service";
import { Observable } from "rxjs/Observable";
import { BobService } from "./../../services/api/bob.service";
import { Person } from "./../../models";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: 'content-bob',
    templateUrl: 'content-bob.component.html'
})

export class ContentBobComponent implements OnInit, OnDestroy {
    
    //Properties
    private activePersonSubscription: Subscription;
    public person: Person|null;
    public certificates: Observable<BOBCertificate[]>;



    //Lifecycle hooks
    constructor(private bobService: BobService, private peopleService: PeopleService) {

    }

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(
            person => this.personChanged(person));
    }
    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
    }


    //Template methods
    public hasData(): boolean {
        return true; //vm.vsbInfo.bobInfo == null || vm.vsbInfo.bobInfo.certificates == null
    }


    //Private Methods
    private personChanged(person: Person|null) {
        this.person = person;
        this.loadData();
    }

    private loadData() {
        if (this.person != null)
        this.certificates = this.bobService.getCertificates(this.person.siNumber);
    }

    
}