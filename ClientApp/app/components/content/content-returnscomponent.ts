import {
    List
} from 'linqts';
import {
    Observable
} from "rxjs/Observable";
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
    BobService,
    ReturnsService,
    ThabService
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
    ZVZWarranty,
    Payment,
    ReturnCalculationRequest,
    ReturnCalculationPayment,
    ReturnCalculationLine,
    ReturnCalculationKind,
    ReturnCalculationResponse
} from "../../models/index";


@Component({
    selector: 'content-returns',
    templateUrl: 'content-returns.component.html'
})
export class ContentReturnsComponent implements OnInit, OnDestroy {


    //Properties
    private activePersonSubscription: Subscription;
    private activePersonDetailSubscription: Subscription;
    private returnCalculationSubscription: Subscription;
    private person: PersonModel | null;

    public returnItem: ReturnCalculationRequest = new ReturnCalculationRequest();
    public returnItemResponse: ReturnCalculationResponse | null = null;
    public reasonList: ReturnReason[] = [];

    public zvzWarranty: Observable < ZVZWarranty[] > ;
    public zvzPayments: Observable < ZVZPayment[] > ;
    public bobPayments: Observable < BOBPayment[] > ;
    public thabPayments: Observable < THABPayment[] > ;

    public totalAmountToReturn: number;
    public ReturnCalculationKind = ReturnCalculationKind;


    //Lifecycle hooks
    constructor(
        private peopleService: PeopleService,
        private zvzService: ZvzService,
        private bobService: BobService,
        private thabService: ThabService,
        private notificationService: NotificationService,
        private translationService: TranslateService,
        private snackBar: MatSnackBar,
        private returnsService: ReturnsService) {}


    public ngOnInit(): void {
        this.fillReturnList();
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personSelected(person));
        this.activePersonDetailSubscription = this.peopleService.activePersonFullDetails.subscribe(person => this.personSelectedDetails(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonDetailSubscription.unsubscribe();
        if (this.returnCalculationSubscription) this.returnCalculationSubscription.unsubscribe();
    }

    private personSelected(person: PersonModel) {
        this.person = person;

        if (this.person == null) {
            this.cancel();
        } else {
            this.zvzPayments = this.zvzService.getPayments(this.person.siNumber);
            this.bobPayments = this.bobService.getPayments(this.person.siNumber);
            this.zvzWarranty = this.zvzService.getWarranties(this.person.siNumber);
        }
    }

    private personSelectedDetails(person: PersonModel) {
        if (this.person == null) return;

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

    private _step: number;
    get currentStep() {
        return this._step;
    }
    set currentStep(index: number) {
        if (this.checkStep(index)) {
            this._step = index;
        }
    }

    public checkStep(stepNumber: number): boolean {
        switch (stepNumber) {
            case 1:
                break;
            case 2:
                if (this.returnItem === null) return false;
                if (this.returnItem.isFraude == undefined) return false;
                if (this.returnItem.isError == undefined) return false;
                if (this.returnItem.reason == undefined) return false;
                if (this.returnItem.reason == "") return false;
                if (this.returnItem.reason == "VALUES_REASON_OTHER") {
                    if (this.returnItem.otherReason == undefined) return false;
                    if (this.returnItem.otherReason == "") return false;
                }
                break;
            case 3:
                return this.returnItemResponse != null && this.returnItemResponse != undefined;
            default:
                return false;
        }
        return true;
    }



    public cancel() {
        this.returnItem = new ReturnCalculationRequest();
    }



    public selectPayment(payment: Payment, kind: ReturnCalculationKind) {
        let paymentLine: ReturnCalculationPayment = new ReturnCalculationPayment(payment.amount, payment.unCode, payment.beginDate, payment.endDate, payment.sendDate);

        let lines = new List < ReturnCalculationLine > (this.returnItem.returnLines)
        if (lines.Any(line => line.kind == kind)) {
            lines.First(line => line.kind == kind).paymentLines.push(paymentLine);
        } else {
            let newLine: ReturnCalculationLine = new ReturnCalculationLine();
            newLine.kind = kind;
            newLine.paymentLines.push(paymentLine);
            this.returnItem.returnLines.push(newLine);
        }
        this.calcTotalAmountToReturn();
    }

    public isPaymentSelected(payment: Payment, kind: ReturnCalculationKind) {
        let paymentLine: ReturnCalculationPayment = new ReturnCalculationPayment(payment.amount, payment.unCode, payment.beginDate, payment.endDate, payment.sendDate);
        let lines = new List < ReturnCalculationLine > (this.returnItem.returnLines);
        let line = lines.FirstOrDefault(line => line.kind == kind);
        if (line == null || line == undefined) return false;
        let paymentLines = new List < ReturnCalculationPayment > (line.paymentLines);
        return paymentLines.Any(p => {
            if (p.amount !== payment.amount) return false;
            if (p.startDate !== payment.beginDate) return false;
            if (p.endDate !== payment.endDate) return false;
            if (p.sendDate !== payment.sendDate) return false;
            if (p.unCode !== payment.unCode) return false;
            return true;
        });
    }

    private calcTotalAmountToReturn() {
        this.totalAmountToReturn = 0;
        for (let line of this.returnItem.returnLines) {
            for (let payment of line.paymentLines) {
                this.totalAmountToReturn += payment.amount;
            }
        }
        this.getCalculationRequest();
    }

    private getCalculationRequest() {
        this.returnCalculationSubscription = this.returnsService.getReturnCalculation(this.returnItem)
            .subscribe(data => this.returnItemResponse = data);
    }
}