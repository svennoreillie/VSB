import { THABNotification } from "./../../models/thab/notification";
import {
    ThabService
} from "./../../services/api/thab.service";
import {
    Observable
} from "rxjs/Observable";
import {
    Subscription
} from "rxjs/Subscription";
import {
    PeopleService
} from "./../../services/api/people.service";
import {
    Component,
    OnInit,
    OnDestroy
} from "@angular/core";
import {
    PersonModel,
    THABCertificate,
    THABPayableAmount,
    THABPayment
} from "../../models";

@Component({
    selector: 'content-thab',
    templateUrl: 'content-thab.component.html'
})
export class ContentThabComponent implements OnInit, OnDestroy {


    //Properties
    public personDetailsLoading: boolean = false;
    public notificationSub: Subscription;
    public remarkSub: Subscription;
    public activePersonSubscription: Subscription;
    public activePersonFullDetailSubscription: Subscription;
    public downloadBob: Subscription;

    public person: PersonModel | null;
    public payments: Observable < THABPayment[] > ;
    public selectedCertificate: THABCertificate;
    public certificateNotification: Observable<THABNotification>;
    public payableAmounts: Observable < THABPayableAmount[] > ;
    public certificates: Observable < THABCertificate[] > ;



    //Lifecycle hooks
    constructor(private thabService: ThabService,
        private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson
            .subscribe(person => this.personChanged(person));
        this.activePersonFullDetailSubscription = this.peopleService.activePersonFullDetails
            .subscribe(person => this.fullDetailsLoaded(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonFullDetailSubscription.unsubscribe();
        if (this.notificationSub) this.notificationSub.unsubscribe();
        if (this.remarkSub) this.remarkSub.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public selectCertificate(certificate: THABCertificate) {
        this.selectedCertificate = certificate;
        if (this.person != null && certificate) {
            this.payableAmounts = this.thabService.getPayableAmounts(this.person.siNumber, certificate.certificateId);
        }
    }

    public setCertificatePopover(certificate: THABCertificate) {
        this.certificateNotification = this.thabService.getNotifications(certificate.certificateId)
                                                       .shareReplay(1);
    }



    //Private methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
        this.personDetailsLoading = true;
    }

    private fullDetailsLoaded(person: PersonModel | null) {
        this.person = person;
        this.personDetailsLoading = false;
        this.loadDetailData();
    }

    private loadDetailData() {
        if (this.person == null) return;

        this.certificates = this.thabService.getCertificates(this.person.siNumber, this.person.insz)
            .map(data => {
                data.forEach(element => {
                    element.initialRemark = element.remark;
                })
                return data;
            })
            .share();
        this.payments = this.thabService.getPayments(this.person.siNumber, this.person.insz)
            .share();

    }

}

