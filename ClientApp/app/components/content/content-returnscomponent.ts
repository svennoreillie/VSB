import { List } from 'linqts';
import {
    Payment
} from "./../../models/payment";
import {
    ThabService
} from "./../../services/api/thab.service";
import {
    Observable
} from "rxjs/Observable";
import {
    ReturnCalculationRequest,
    ReturnCalculationLine,
    ReturnCalculationKind,
    ReturnCalculationPayment
} from "./../../models/returns/returncalculationrequest";
import {
    Subscription
} from "rxjs/Subscription";
import {
    Component,
    OnDestroy,
    OnInit
} from "@angular/core";
import {
    NotificationService,
    PeopleService,
    ZvzService,
    BobService
} from "../../services/index";
import {
    TranslateService
} from "../../directives/index";
import {
    MatSnackBar
} from "@angular/material";
import {
    PersonModel,
    ReturnReason,
    ZVZPayment,
    BOBPayment,
    THABPayment,
    ZVZWarranty
} from "../../models/index";


@Component({
    selector: 'content-returns',
    templateUrl: 'content-returns.component.html'
})
export class ContentReturnsComponent implements OnInit, OnDestroy {

    //Properties
    private activePersonSubscription: Subscription;
    private activePersonDetailSubscription: Subscription;
    private person: PersonModel | null;

    public returnItem: ReturnCalculationRequest = new ReturnCalculationRequest();
    public reasonList: ReturnReason[] = [];
    public zvzWarranty: Observable < ZVZWarranty[] > ;
    public zvzPayments: Observable < ZVZPayment[] > ;
    public bobPayments: Observable < BOBPayment[] > ;
    public thabPayments: Observable < THABPayment[] > ;
    public selectedPayments: any[] = [];
    public totalAmountToReturn: number;
    

    //Lifecycle hooks
    constructor(
        private peopleService: PeopleService,
        private zvzService: ZvzService,
        private bobService: BobService,
        private thabService: ThabService,
        private notificationService: NotificationService,
        private translationService: TranslateService,
        private snackBar: MatSnackBar) {}


    public ngOnInit(): void {
        this.fillReturnList();
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personSelected(person));
        this.activePersonDetailSubscription = this.peopleService.activePersonFullDetails.subscribe(person => this.personSelectedDetails(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonDetailSubscription.unsubscribe();
    }

    private personSelected(person: PersonModel) {
        this.person = person;
        this.zvzPayments = this.zvzService.getPayments(this.person.siNumber);
        this.bobPayments = this.bobService.getPayments(this.person.siNumber);
        this.zvzWarranty = this.zvzService.getWarranties(this.person.siNumber);
    }

    private personSelectedDetails(person: PersonModel) {
        this.person = person;
        this.thabPayments = this.thabService.getPayments(this.person.siNumber, this.person.insz);
    }



    private fillReturnList() {
        this.reasonList = [];
        for (var index = 0; index < 4; index++) {
            this.reasonList.push({
                index: index,
                value: `VALUES_REASON_${index}`,
                displayValue: `ReturnReason${index}`
            });
        }
        this.reasonList.push({
            index: 4,
            value: "VALUES_REASON_OTHER",
            displayValue: "ReturnReasonOther"
        });
    }


    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public checkStep(stepNumber: number): boolean {
        switch (stepNumber) {
            case 1:
                if (this.returnItem === null) return false;
                if (this.returnItem.IsFraude == undefined) return false;
                if (this.returnItem.IsError == undefined) return false;
                if (this.returnItem.Reason == undefined) return false;
                if (this.returnItem.Reason == "") return false;
                if (this.returnItem.Reason == "VALUES_REASON_OTHER") {
                    if (this.returnItem.OtherReason == undefined) return false;
                    if (this.returnItem.OtherReason == "") return false;
                }
                break;
            case 2:
                break;
            default:
                return false;
        }
        return true;
    }



    public cancelReturnItem() {
        this.returnItem = new ReturnCalculationRequest();
    }


    public selectZVZPayment(payment: ZVZPayment) {
        let paymentLine: ReturnCalculationPayment = new ReturnCalculationPayment(payment.amount, payment.unCode, payment.beginDate, payment.endDate, payment.sendDate);

        let lines = new List<ReturnCalculationLine>(this.returnItem.ReturnLines)
        if (lines.Any(line => line.Kind == ReturnCalculationKind.ZVZ)) {
            lines.First(line => line.Kind == ReturnCalculationKind.ZVZ).PaymentLines.push(paymentLine);
        } else {
            let newLine: ReturnCalculationLine = new ReturnCalculationLine();
            newLine.Kind = ReturnCalculationKind.ZVZ;
            newLine.PaymentLines.push(paymentLine);
            this.returnItem.ReturnLines.push(newLine);
        }
        this.calcTotalAmountToReturn();
    }

    public selectBOBPayment(payment: BOBPayment) {
        let paymentLine: ReturnCalculationPayment = new ReturnCalculationPayment(payment.amount, payment.unCode, payment.beginDate, payment.endDate, payment.sendDate);

        let lines = new List<ReturnCalculationLine>(this.returnItem.ReturnLines)
        if (lines.Any(line => line.Kind == ReturnCalculationKind.BOB)) {
            lines.First(line => line.Kind == ReturnCalculationKind.BOB).PaymentLines.push(paymentLine);
        } else {
            let newLine: ReturnCalculationLine = new ReturnCalculationLine();
            newLine.Kind = ReturnCalculationKind.BOB;
            newLine.PaymentLines.push(paymentLine);
            this.returnItem.ReturnLines.push(newLine);
        }
        this.calcTotalAmountToReturn();
    }

    public selectTHABPayment(payment: THABPayment) {
        let paymentLine: ReturnCalculationPayment = new ReturnCalculationPayment(payment.Amount, payment.UnCode, payment.PeriodStart, payment.PeriodEnd, payment.SendDate);

        let lines = new List<ReturnCalculationLine>(this.returnItem.ReturnLines)
        if (lines.Any(line => line.Kind == ReturnCalculationKind.THAB)) {
            lines.First(line => line.Kind == ReturnCalculationKind.THAB).PaymentLines.push(paymentLine);
        } else {
            let newLine: ReturnCalculationLine = new ReturnCalculationLine();
            newLine.Kind = ReturnCalculationKind.THAB;
            newLine.PaymentLines.push(paymentLine);
            this.returnItem.ReturnLines.push(newLine);
        }
        this.calcTotalAmountToReturn();
    }

    public isPaymentSelected(payment: Payment) {
        return this.selectedPayments.indexOf(payment) >= 0;
    }

    private calcTotalAmountToReturn() {
        this.totalAmountToReturn = 0;
        for (let line of this.returnItem.ReturnLines) {
            for (let payment of line.PaymentLines) {
                this.totalAmountToReturn += payment.Amount;
            }
        }
        this.getCalculationRequest();
    }

}