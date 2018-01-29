import {
    List
} from "linqts";
import {
    Component,
    OnInit,
    OnDestroy,
    Input
} from '@angular/core';
import {
    StepBaseComponent
} from './step-base.component';
import {
    PeopleService,
    ZvzService,
    BobService,
    ThabService
} from '../../../services/index';
import {
    PersonModel, THABPayment, BOBPayment, ZVZPayment, ZVZWarranty, ReturnCalculationPayment, Payment,
    ReturnCalculationKind,
    ReturnCalculationLine,
    ReturnCalculationRequest
} from '../../../models/index';
import {
    Observable, Subscription
} from 'rxjs';

@Component({
    selector: 'step-registration-calculation',
    templateUrl: 'step-registration-calculation.component.html'
})

export class StepRegistrationCalculationComponent implements StepBaseComponent, OnInit, OnDestroy {

    private zvzWarrantySub: Subscription;
    private activePersonDetailSubscription: any;
    private activePersonSubscription: any;

    @Input() public returnItem: ReturnCalculationRequest;

    public person: PersonModel;
    public ReturnCalculationKind = ReturnCalculationKind;
    public zvzWarranties: ZVZWarranty[];
    public zvzPayments: Observable<ZVZPayment[]>;
    public bobPayments: Observable<BOBPayment[]>;
    public thabPayments: Observable<THABPayment[]>;
    public thabFodPayments: Observable<THABPayment[]>;


    //Lifecycle hooks
    constructor(
        private peopleService: PeopleService,
        private zvzService: ZvzService,
        private bobService: BobService,
        private thabService: ThabService) { }


    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personSelected(person));
        this.activePersonDetailSubscription = this.peopleService.activePersonFullDetails.subscribe(person => this.personSelectedDetails(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonDetailSubscription.unsubscribe();
        if (this.zvzWarrantySub) this.zvzWarrantySub.unsubscribe();
    }


    public activate(): void {
        this.zvzPayments = this.zvzService.getPayments(this.person.siNumber);
        this.bobPayments = this.bobService.getPayments(this.person.siNumber);
        this.zvzWarrantySub = this.zvzService.getWarranties(this.person.siNumber).subscribe(data => this.zvzWarranties = data);
        this.thabPayments = this.thabService.getPayments(this.person.siNumber, this.person.insz);
        this.thabFodPayments = this.thabService.getFODPayments(this.person.siNumber, this.person.insz);
    }

    public isCompleted(): boolean {
        return this.returnItem && this.returnItem.returnLines && this.returnItem.returnLines.length > 0;
    }

    public deactivate(): void {

    }

    public getZvzCareForm(payment: ZVZPayment) {
        let list = new List<ZVZWarranty>(this.zvzWarranties);
        let warranty = list.FirstOrDefault(zw => zw.certificate == payment.certificateId);
        if (warranty) return warranty.careForm;

    }

    public selectPayment(payment: Payment, kind: ReturnCalculationKind) {
        if (!this.returnItem) return;
        let lines = new List<ReturnCalculationLine>(this.returnItem.returnLines)
        if (lines.Any(line => line.kind == kind)) {
            if (this.isPaymentSelected(payment, kind)) {
                let paymentLines = new List<ReturnCalculationPayment>(lines.First(line => line.kind == kind).paymentLines);
                let line = paymentLines.FirstOrDefault(p => this.isEqualPayment(p, payment));
                paymentLines.Remove(line);
                if (!paymentLines.Any()) {
                    let returnLine = lines.First(line => line.kind == kind);
                    lines.Remove(returnLine);
                }
            }
            else lines.First(line => line.kind == kind).paymentLines.push(this.createPayment(payment));
        } else {
            let newLine: ReturnCalculationLine = new ReturnCalculationLine();
            newLine.kind = kind;
            newLine.paymentLines.push(this.createPayment(payment));
            this.returnItem.returnLines.push(newLine);
        }
    }

    public isPaymentSelected(payment: Payment, kind: ReturnCalculationKind) {
        if (!this.returnItem) return;
        let lines = new List<ReturnCalculationLine>(this.returnItem.returnLines);
        let line = lines.FirstOrDefault(line => line.kind == kind);
        if (line == null || line == undefined) return false;
        let paymentLines = new List<ReturnCalculationPayment>(line.paymentLines);
        return paymentLines.Any(p => this.isEqualPayment(p, payment));
    }




    //privates
    private personSelected(person: PersonModel) {
        if (!this.person || !this.person.insz) this.person = person;
    }

    private personSelectedDetails(person: PersonModel) {
        this.person = person;
    }

    private createPayment(payment: Payment): ReturnCalculationPayment {
        return new ReturnCalculationPayment(payment.amount, payment.unCode, payment.beginDate, payment.endDate, payment.sendDate);
    }

    private isEqualPayment(p1: ReturnCalculationPayment, p2: Payment) {
        if (p1.amount !== p2.amount) return false;
        if (p1.startDate !== p2.beginDate) return false;
        if (p1.endDate !== p2.endDate) return false;
        if (p1.sendDate !== p2.sendDate) return false;
        if (p1.unCode !== p2.unCode) return false;
        return true;
    }
}