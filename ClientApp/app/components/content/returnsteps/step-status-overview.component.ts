import { StepBaseComponent } from "./step-base.component";
import {
    Component,
    OnInit,
    Input,
    OnDestroy
} from '@angular/core';
import {
    ReturnCalculationRequest,
    SelectItem,
    ReturnCalculationResponse
} from '../../../models/index';
import { ReturnsService, GeneralDataService, PeopleService } from "../../../services/index";
import { Subscription } from "rxjs";
import { PersonModel } from "../../../models/Person";
import { TranslateService } from "../../../directives/index";

@Component({
    selector: 'step-status-overview',
    templateUrl: 'step-status-overview.component.html'
})

export class StepStatusOverviewComponent implements StepBaseComponent, OnDestroy {

    //Subscriptions 
    private returnCalculationSubscription: Subscription;
    private regularSub: Subscription;
    private personSub: Subscription;
    private userSub: Subscription;
    private signedSub: Subscription;

    private user: string;

    public person: PersonModel;
    public returnItemResponse: ReturnCalculationResponse;
    public reworkingProposition: boolean = true;
    public reactionRequired: boolean = true;
    public splitlist: any[];
    
    @Input() public returnItem: ReturnCalculationRequest;


    constructor(private returnsService: ReturnsService,
        private generalService: GeneralDataService,
        private peopleService: PeopleService,
        private translateService: TranslateService) {

        this.splitlist = new Array<SelectItem>();

        let pre = translateService.getReplacementValue("OGMSPLIT_PRE");
        let post = translateService.getReplacementValue("OGMSPLIT_POST");

        for (var i = 0; i < 12; i++) {
            this.splitlist.push(new SelectItem(i, i+1, `${pre} ${i+1} ${post}`));
        }
    }

    public ngOnDestroy(): void {
        if (this.personSub) this.personSub.unsubscribe();
        if (this.userSub) this.userSub.unsubscribe();
        if (this.regularSub) this.regularSub.unsubscribe();
        if (this.signedSub) this.signedSub.unsubscribe();
        if (this.returnCalculationSubscription) this.returnCalculationSubscription.unsubscribe();
    }

    public activate() {
        this.userSub = this.generalService.getUser().subscribe(data => this.user = data.user);
        this.personSub = this.peopleService.activePersonFullDetails.subscribe(person => this.person = person);

        this.returnCalculationSubscription = this.returnsService.getReturnCalculation(this.returnItem)
            .subscribe(data => this.returnItemResponse = data);
    }

    public deactivate() {
    }

    public isCompleted(): boolean {
        return false
    }

    public signedLetterDisabled() {
        if (!this.returnItem) return true;
        if (this.returnItem.signedLetterCreateDate) return true;
        if (!this.returnItem.regularLetterCreateDate) return true;
        let duration = (new Date()).valueOf() - this.returnItem.regularLetterCreateDate.valueOf();
        if (duration > (30 * 24 * 60 * 60 * 1000)) return false;
        return true;
    }

    public sendSignedLetter = () => {
        if (!this.returnItem.regularLetterCreateDate) return;
        if (this.returnItem.signedLetterCreateDate) return;
        this.returnItem.signedLetterCreateDate = new Date();
        this.returnItem.signedLetterCreatedBy = this.user;
        this.signedSub = this.returnsService.postSignedLetter(this.returnItem)
            .subscribe(data => this.reactionRequired = false, error => console.error(error));
    }

    public canAlterProposal() {
        if (!this.returnItemResponse) return false;
        return this.returnItemResponse.amountRefundableByOGM > 0;
    }

    public reworkProposition() {
        this.reworkingProposition = true;
        this.reactionRequired = false;
    }

    public saveReworkedProposition() {
        //TODO :: save to server
        this.reworkingProposition = false;
    }

    public disagree() {
        //TODO :: save to server??
        this.reactionRequired = false;
    }
   
}