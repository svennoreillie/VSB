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
    }



    //Template methods
    public hasData(): boolean {
        return true; //vm.vsbInfo.bobInfo == null || vm.vsbInfo.bobInfo.certificates == null
    }

    public downloadBobForm() {
        console.error("todo download bob");
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

// import {
//     Subscription
// } from "rxjs/Subscription";
// import {
//     BOBLetter
// } from "./../../models/bob/letter";
// import {
//     BOBPayment
// } from "./../../models/bob/payment";
// import {
//     BOBCertificate
// } from "./../../models/bob/certificate";
// import {
//     PeopleService
// } from "./../../services/api/people.service";
// import {
//     BobService
// } from "./../../services/api/bob.service";
// import {
//     PersonModel
// } from "./../../models";
// import {
//     Component,
//     OnInit,
//     OnDestroy
// } from "@angular/core";

// @Component({
//     selector: 'content-bob',
//     templateUrl: 'content-bob.component.html'
// })
// export class ContentBobComponent implements OnInit, OnDestroy {

//     //Properties
//     public activePersonSubscription: Subscription;
//     public person: PersonModel | null;

//     private certificatesSubscription: Subscription;
//     public certificatesLoading: boolean;
//     public certificates: BOBCertificate[];

//     private paymentsSubscription: Subscription;
//     public paymentsLoading: boolean;
//     public payments: BOBPayment[];

//     private lettersSubscription: Subscription;
//     public lettersLoading: boolean;
//     public letters: BOBLetter[];


//     //Lifecycle hooks
//     constructor(private bobService: BobService, private peopleService: PeopleService) {}

//     public ngOnInit(): void {
//         this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personChanged(person));
//     }

//     public ngOnDestroy(): void {
//         this.activePersonSubscription.unsubscribe();
//         if (this.certificatesSubscription) this.certificatesSubscription.unsubscribe();
//         if (this.lettersSubscription) this.lettersSubscription.unsubscribe();
//         if (this.paymentsSubscription) this.paymentsSubscription.unsubscribe();
//     }



//     //Template methods
//     public hasData(): boolean {
//         return true; //vm.vsbInfo.bobInfo == null || vm.vsbInfo.bobInfo.certificates == null
//     }

//     public downloadBobForm() {
//         console.error("todo download bob");
//     }


//     //Private Methods
//     private personChanged(person: PersonModel | null) {
//         this.person = person;
//         this.loadData();
//     }

//     private loadData() {
//         if (this.person == null) return;

//         if (this.certificatesSubscription) this.certificatesSubscription.unsubscribe();
//         this.certificatesLoading = true;
//         this.certificatesSubscription = this.bobService.getCertificates(this.person.siNumber)
//             .subscribe(data => this.certificates = data,
//                 (error: any) => console.error(error),
//                 () => this.certificatesLoading = false);


//         this.paymentsLoading = true;
//         this.paymentsSubscription = this.bobService.getPayments(this.person.siNumber)
//             .subscribe(data => this.payments = data,
//                 (error: any) => console.error(error),
//                 () => this.paymentsLoading = false);

//         this.lettersLoading = true;
//         this.lettersSubscription = this.bobService.getLetters(this.person.siNumber)
//             .subscribe(data => this.letters = data,
//                 (error: any) => console.error(error),
//                 () => this.lettersLoading = false);
//     }

// }