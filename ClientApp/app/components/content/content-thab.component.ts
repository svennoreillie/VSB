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
    THABNotification,
    THABPayment,
    THABLetter
} from "../../models";

@Component({
    selector: 'content-thab',
    templateUrl: 'content-thab.component.html'
})
export class ContentThabComponent implements OnInit, OnDestroy {
    downloadCalculationSub: Subscription;

    //Properties
    private notificationSub: Subscription;
    private certificateLettersSub: Subscription;
    private remarkSub: Subscription;
    private activePersonSubscription: Subscription;
    private activePersonFullDetailSubscription: Subscription;
    private downloadBobSub: Subscription;

    public extraInfoLetterType: string = "Opvragen extra gegevens THAB";
    public personDetailsLoading: boolean = false;
    public loading: boolean;
    public person: PersonModel | null;
    public payments: Observable < THABPayment[] > ;
    public selectedCertificate: THABCertificate;
    public certificateNotification: Observable < THABNotification > ;
    public payableAmounts: Observable < THABPayableAmount[] > ;
    public certificates: THABCertificate[];
    public letters: THABLetter[];

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
        if (this.certificateLettersSub) this.certificateLettersSub.unsubscribe();
        if (this.downloadBobSub) this.downloadBobSub.unsubscribe();
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

    public saveThabRemark(certificate: THABCertificate) {
        if (this.person != null) {
            this.thabService.saveRemark(this.person.siNumber, certificate)
                .subscribe(data => data,
                    error => console.error(error));
        }
    }

    public downloadThabForm(letter: THABLetter) {
        let cert = this.certificates.filter((c: THABCertificate) => c.certificateId == letter.certificateId)[0];
        if (this.person !== null && cert !== null) {
            this.downloadBobSub = this.thabService.downloadForm(this.person.siNumber, cert.referenceDate)
                .subscribe(blob => {
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    if (this.person != null) link.download = `Opvragen gegevens ${this.person.firstName} ${this.person.name}.docx`;
                    link.click();
                }, error => console.error(error));
        }
    }

    public downloadThabCalculationDocument(certificate: THABCertificate) {
        if (this.person !== null) {
            this.downloadCalculationSub = this.thabService.downloadCalculation(this.person.siNumber, certificate.certificateId, this.person.insz)
                .subscribe(blob => {
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    if (this.person != null) link.download = `ThabCalculation ${this.person.firstName} ${this.person.name}.pdf`;
                    link.click();
                }, error => console.error(error));
        }
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
        this.payments = this.thabService.getPayments(this.person.siNumber, this.person.insz);

        this.loading = true;
        this.certificateLettersSub = Observable.forkJoin(
            this.thabService.getCertificates(this.person.siNumber, this.person.insz),
            this.thabService.getLetters(this.person.siNumber, this.person.insz)
        ).subscribe(data => {
            this.certificates = data[0];

            this.certificates.forEach(element => {
                element.initialRemark = element.remark;
            })
            if (this.certificates.length == 1) this.selectCertificate(this.certificates[0]);

            this.letters = data[1];
        }, error => error,
         () => {
            this.createExtraInfoLetters();            
            this.loading = false
        });
    }

    private createExtraInfoLetters(): void {
        if (this.certificates) {
            let letters = this.certificates.filter(c => c.state == "In afwachting");
            if (letters.length > 0) {
                var extraLetters = letters.map(c => new THABLetter(c.certificateId, this.extraInfoLetterType));

                if (this.letters === undefined) this.letters = extraLetters;
                else this.letters = extraLetters.concat(this.letters);
            }
        }
    }
}