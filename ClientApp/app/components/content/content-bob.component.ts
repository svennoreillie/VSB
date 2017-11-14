import { PersonModel } from "./../../models/person";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { BOBLetter } from "./../../models/bob/letter";
import { BOBPayment } from "./../../models/bob/payment";
import { BOBCertificate } from "./../../models/bob/certificate";
import { PeopleService } from "./../../services/api/people.service";
import { BobService } from "./../../services/api/bob.service";
import { Component, OnInit, OnDestroy } from "@angular/core";

@Component({
    selector: 'content-bob',
    templateUrl: 'content-bob.component.html'
})
export class ContentBobComponent implements OnInit, OnDestroy {

    //Properties
    public activePersonSubscription: Subscription;
    public downloadBob: Subscription;

    public person: PersonModel | null;

    public certificates: Observable<BOBCertificate[]>;
    public payments: Observable<BOBPayment[]>;
    public letters: Observable<BOBLetter[]>;


    //Lifecycle hooks
    constructor(private bobService: BobService, private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personChanged(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        if (this.downloadBob) this.downloadBob.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public downloadBobForm() {
        if (this.person !== null) {
            this.downloadBob = this.bobService.downloadBobForm(this.person.siNumber)
                                              .subscribe(blob => {
                                                  var link = document.createElement('a');
                                                  link.href = window.URL.createObjectURL(blob);
                                                  if (this.person != null) link.download = `aanvraag formulier bob voor ${this.person.firstName} ${this.person.name}.pdf`;
                                                  link.click();
                                              }, error => console.error(error));
        }
    }


    //Private Methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
        this.loadData();
    }

    private loadData() {
        if (this.person == null) return;

        this.certificates = this.bobService.getCertificates(this.person.siNumber).share();
        this.payments = this.bobService.getPayments(this.person.siNumber).share();
        this.letters = this.bobService.getLetters(this.person.siNumber).share();
    }

}
