import { ActiveContentPageService } from "./../../services/activecontent/active-content-page.service";
import { StepBaseComponent } from "./returnsteps/step-base.component";
import { List } from 'linqts';
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { Component, OnDestroy, OnInit, ViewChild, forwardRef } from "@angular/core";
import { NotificationService, PeopleService, ZvzService, BobService, ReturnsService, ThabService } from "../../services/index";
import { TranslateService } from "../../directives/index";
import { MatSnackBar } from "@angular/material";
import { PersonModel, SelectItem, ZVZPayment, BOBPayment, THABPayment, ZVZWarranty, Payment, ReturnCalculationRequest, ReturnCalculationPayment, ReturnCalculationLine, ReturnCalculationKind, ReturnCalculationResponse } from "../../models/index";
import { StepRegistrationComponent } from './returnsteps/step-registration.component';
import { StepRegistrationCalculationComponent, StepStatusOverviewComponent } from './index';
import { SocStepperChange } from '../soc-stepper/soc-stepper.component';
import { StepPropositionComponent } from "./returnsteps/step-proposition.component";


@Component({
    selector: 'content-returns',
    templateUrl: 'content-returns.component.html'
})
export class ContentReturnsComponent implements OnInit, OnDestroy {
    

    private activePersonSubscription: Subscription;
    private activePersonDetailSubscription: Subscription;
    private returnCalculationSubscription: Subscription;
    private person: PersonModel | null;

    public returnItem: ReturnCalculationRequest = new ReturnCalculationRequest();
    public currentStep: number;

    @ViewChild(StepRegistrationComponent) stepRegistration: StepRegistrationComponent;
    @ViewChild(forwardRef(() => StepRegistrationCalculationComponent)) stepRegistrationCalculation: StepRegistrationCalculationComponent;
    @ViewChild(forwardRef(() => StepPropositionComponent)) stepProposition: StepPropositionComponent;
    @ViewChild(forwardRef(() => StepStatusOverviewComponent)) stepStatusOverview: StepStatusOverviewComponent;


    //Lifecycle hooks
    constructor(private peopleService: PeopleService) { }

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personSelected(person));
        this.activePersonDetailSubscription = this.peopleService.activePersonFullDetails.subscribe(data => this.personDetailsChanged(data));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonDetailSubscription.unsubscribe();
    }


    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public checkStep(stepIndex: number): boolean {
        switch (stepIndex) {
            case 0:
                return true;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                return this.isStepCompleted(stepIndex- 1);
            default:
                return false;
        }
    }

    public stepChanged(change: SocStepperChange) {
        this.getStep(change.oldIndex).deactivate();
        this.getStep(change.newIndex).activate();
    }

    public setNextStep(step: number) {
        this.currentStep = step;
    }


    // Privates
    private personSelected(person: PersonModel) {
        this.person = person;

        if (this.person == null) {
            this.returnItem = new ReturnCalculationRequest();
        } 
    }

    private personDetailsChanged(person: PersonModel) {
        if (person == null) return;
        this.returnItem.siNumber = person.siNumber;
        this.returnItem.federationNumber = person.federationNumber;
        this.returnItem.insz = person.insz;
    }

    private isStepCompleted(stepIndex: number) {
        let step = this.getStep(stepIndex);
        if (!step) return false;
        return step.isCompleted();
    }

    private getStep(index: number): StepBaseComponent {
        switch (index) {
            case 0:
                return this.stepRegistration;
            case 1:
                return this.stepRegistrationCalculation;
            case 2:
                return this.stepProposition;
            case 3:
                return this.stepStatusOverview;
            default:
                break;
        }
    }
}