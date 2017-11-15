import { THABCertificate } from "./../../models/thab/certificate";
import { ThabService } from "./../../services/api/thab.service";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { PeopleService } from "./../../services/api/people.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { PersonModel, THABCertificate } from "../../models";

@Component({
    selector: 'content-thab',
    templateUrl: 'content-thab.component.html'
})
export class ContentThabComponent implements OnInit, OnDestroy {
    selectedCertificate: THABCertificate;
    payableAmounts: Observable<THABPayableAmount[]>;
    
    //Properties
    public activePersonSubscription: Subscription;
    public activePersonFullDetailSubscription: Subscription;
    public downloadBob: Subscription;

    public person: PersonModel | null;

    public certificates: Observable<THABCertificate[]>;



    //Lifecycle hooks
    constructor(private thabService: ThabService, private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson
        .subscribe(person => this.personChanged(person));
    this.activePersonFullDetailSubscription = this.peopleService.activePersonFullDetails
        .subscribe(person => this.fullDetailsLoaded(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonFullDetailSubscription.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public getRemark(certificate: THABCertificate) {
        if (this.person != null) 
        return this.thabService.getRemark(this.person.siNumber, certificate);
    }

    public selectCertificate(certificate: THABCertificate) {
        this.selectedCertificate = certificate;
        if (this.person != null && certificate) {
            this.payableAmounts= this.thabService.getPayableAmounts(this.person.siNumber, certificate.certificateId);
        }
    }


    //Private methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
    }
    
    private fullDetailsLoaded(person: PersonModel | null) {
        this.person = person;
        this.loadDetailData();
    }
    
    private loadDetailData() {
        if (this.person == null) return;

        this.certificates = this.thabService.getCertificates(this.person.siNumber, this.person.insz)
                                            .share();
    }

}
