import { StepBaseComponent } from "./step-base.component";
import {
    Component,
    OnInit,
    Input,
    OnDestroy,    OnChanges,
    SimpleChanges
} from '@angular/core';
import {
    ReturnCalculationRequest,
    SelectItem,
    ReturnCalculationResponse
} from '../../../models/index';
import { ReturnsService } from "../../../services/index";
import { Subscription } from "rxjs";

@Component({
    selector: 'proposition',
    templateUrl: 'proposition.component.html'
})
export class PropositionComponent implements OnDestroy, OnChanges {
    
    private returnCalculationSubscription: Subscription;
    private _returnItem: ReturnCalculationRequest = undefined;
    public calculationLoading: boolean;
    public returnItemResponse: ReturnCalculationResponse | null = null;

    @Input() public collapsed: boolean = false;
    @Input() public returnItem: ReturnCalculationRequest;
    

    constructor(private returnsService: ReturnsService) {
    }

    public ngOnDestroy(): void {
        if (this.returnCalculationSubscription) this.returnCalculationSubscription.unsubscribe();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.getCalculationRequest();
    }
    
    private getCalculationRequest() {
        if (!this._returnItem) return;
        this.calculationLoading = true;
        this.returnCalculationSubscription = this.returnsService.getReturnCalculation(this.returnItem)
            .subscribe(data => this.returnItemResponse = data, err => { }, () => this.calculationLoading = false);
    }
}