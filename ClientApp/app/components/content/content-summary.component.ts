import {
    TranslateService
} from "./../../directives/translate/translate.service";
import {
    Subscription
} from "rxjs/Subscription";
import {
    PeopleService
} from "./../../services/api/people.service";
import {
    ZvzService
} from "./../../services/api/zvz.service";
import {
    PersonModel,
    ZVZContract,
    THABCertificate,
    BOBCertificate,
    ZVZWarranty,
    ZVZContribution
} from "./../../models";
import {
    Component,
    OnInit,
    Input,
    OnDestroy
} from "@angular/core";
import {
    Observable
} from "rxjs/Observable";
import {
    BobService,
    ThabService
} from "../../services/index";
import {
    VSBService
} from "../../services/api/vsb.service";
import { BOBContact } from "../../models/bob/contact";

@Component({
    selector: 'content-summary',
    templateUrl: 'content-summary.component.html'
})

export class ContentSummaryComponent implements OnInit, OnDestroy {
    private vsbGetRemarkSub: Subscription;
    private bobContactSub: Subscription;
    private vsbRemarkSub: Subscription;
    private personSub: Subscription;
    private personDetailSub: Subscription;
    private contact: BOBContact;

    public thabCertificates: Observable < THABCertificate[] > ;
    public bobCertificates: Observable < BOBCertificate[] > ;
    public warranties: Observable < ZVZWarranty[] > ;
    public lastContract: Observable < ZVZContract > ;
    public contributions: Observable < ZVZContribution[] > ;
    public vsbRemark: string;
    public person: PersonModel | null;

    constructor(private zvzService: ZvzService,
        private peopleService: PeopleService,
        private bobService: BobService,
        private thabService: ThabService,
        private vsbService: VSBService,
        private translateService: TranslateService) {}

    public ngOnInit() {
        this.personSub = this.peopleService.activePerson.subscribe(data => this.personChanged(data));
        this.personDetailSub = this.peopleService.activePersonFullDetails.subscribe(data => this.personDetailsChanged(data));
    }

    public ngOnDestroy() {
        this.personSub.unsubscribe();
        this.personDetailSub.unsubscribe();
        if (this.vsbRemarkSub) this.vsbRemarkSub.unsubscribe();
        if (this.bobContactSub) this.bobContactSub.unsubscribe();
        if (this.vsbGetRemarkSub) this.vsbGetRemarkSub.unsubscribe();
    }


    public createMailTo(): string {
        let subject: string;
        let body: string;

        if (this.person == null) return "";
        var space = " ";
        subject = this.translateService.getReplacementValue("VSB_MAIL_SUBJECT_LEGEND") +
            space + this.person.name + space + this.person.firstName + space + this.person.insz;

        var enter = encodeURI("\r\n");
        var space = " ";

        body = this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_NAME") + space + this.person.name + space + this.person.firstName;
        body += enter;

        body += this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_NISSNUMBER") + space + this.person.insz;
        body += enter;

        body += this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_PHONE") + space;
        body += (this.contact != null &&this.contact.phoneNumber != null) ? this.contact.phoneNumber : this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_EMPTY  ");
        body += enter;

        body += this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_MAIL") + space;
        body += (this.contact != null && this.contact.email != null) ? this.contact.email : this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_EMPTY");
        body += enter;

        body += this.translateService.getReplacementValue("VSB_MAIL_BODY_LINE_REMARK");

        let link: string = "mailto:?subject= " + subject + " &body=" + body + " " + this.vsbRemark;
        return link;
    }


    public saveVsbRemark(): void {
        if (this.vsbRemark && this.person != null) {
            this.vsbRemarkSub = this.vsbService.saveRemark(this.person.siNumber, this.vsbRemark)
                                               .subscribe();
        }
    }



    private personChanged(p: PersonModel | null): void {
        this.person = p;
        if (p != null) {
            this.lastContract = this.zvzService.getContract(p.siNumber);
            this.warranties = this.zvzService.getWarranties(p.siNumber);
            this.bobCertificates = this.bobService.getCertificates(p.siNumber);
            this.contributions = this.zvzService.getContributions(p.siNumber);
            this.bobContactSub = this.bobService.getBOBContact(p.siNumber).subscribe(data => this.contact = data);
            this.vsbGetRemarkSub = this.vsbService.getRemark(p.siNumber).subscribe(data => this.vsbRemark = data);
        }
    }

    private personDetailsChanged(p: PersonModel | null): void {
        this.person = p;
        if (p != null) {
            this.thabCertificates = this.thabService.getCertificates(p.siNumber, p.insz);
        }
    }


}