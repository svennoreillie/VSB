import { StepBaseComponent } from "./step-base.component";
import {
    Component,
    OnInit,
    Input,
    OnDestroy,
    Output,
    EventEmitter
} from '@angular/core';
import {
    ReturnCalculationRequest,
    SelectItem,
    ReturnCalculationResponse,

    ReturnCalculationKind,
    PersonModel
} from '../../../models/index';
import { ReturnsService, GeneralDataService, PeopleService } from "../../../services/index";
import { Subscription } from "rxjs";

@Component({
    selector: 'step-proposition',
    templateUrl: 'step-proposition.component.html'
})

export class StepPropositionComponent implements StepBaseComponent, OnDestroy {
    
    private letterCreated: boolean = false;
    private user: string;
    private personSub: Subscription;
    private userSub: Subscription;
    private regularSub: Subscription;
    private returnCalculationSubscription: Subscription;

    @Input() public returnItem: ReturnCalculationRequest;
    @Output() public nextStep: EventEmitter<any> = new EventEmitter<any>();

    public calculationLoading: boolean = true;
    public totalAmountToReturn: number;
    public returnItemResponse: ReturnCalculationResponse;
    public ReturnCalculationKind = ReturnCalculationKind;
    public person: PersonModel;


    constructor(private returnsService: ReturnsService,
        private generalService: GeneralDataService,
        private peopleService: PeopleService) {

    }

    public ngOnDestroy(): void {
        if (this.returnCalculationSubscription) this.returnCalculationSubscription.unsubscribe();
        if (this.regularSub) this.regularSub.unsubscribe();
        if (this.personSub) this.personSub.unsubscribe();
        if (this.userSub) this.userSub.unsubscribe();
    }

    public activate() {
        this.calcTotalAmountToReturn();        
        this.userSub = this.generalService.getUser().subscribe(data => this.user = data.user);
        this.personSub = this.peopleService.activePersonFullDetails.subscribe(person => this.person = person);
    }

    public deactivate() {
        
    }

    public isCompleted(): boolean {
        return this.returnItemResponse != undefined && this.letterCreated;
    }

    public canAlterProposal(): boolean {
        if (this.totalAmountToReturn == 0) return false;
        if (this.returnItemResponse == undefined) return false;
        if (this.totalAmountToReturn == this.returnItemResponse.amountNonRefundable) return false;
        return true;
    }

    private calcTotalAmountToReturn() {
        if (!this.returnItem) return 0;
        this.totalAmountToReturn = 0;
        for (let line of this.returnItem.returnLines) {
            if (line.kind == ReturnCalculationKind.THAB_FOD) continue;
            for (let payment of line.paymentLines) {
                this.totalAmountToReturn += payment.amount;
            }
        }
        this.getCalculationRequest();
    }

    private getCalculationRequest() {
        this.calculationLoading = true;
        this.returnCalculationSubscription = this.returnsService.getReturnCalculation(this.returnItem)
            .subscribe(data => this.returnItemResponse = data, err => { }, () => this.calculationLoading = false);
    }

    public createRegularLetter = () => {
        if (this.returnItem.regularLetterCreateDate) return;
        this.returnItem.regularLetterCreateDate = new Date();
        this.returnItem.regularLetterCreatedBy = this.user;
        
        this.regularSub = this.returnsService.postRegularLetter(this.returnItem)
            .subscribe(data => {
                this.letterCreated = true;
                this.nextStep.emit(4);
             }, error => console.error(error));
    }

}