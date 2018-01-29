import { StepBaseComponent } from "./step-base.component";
import {
    Component,
    OnInit,
    Input
} from '@angular/core';
import {
    ReturnCalculationRequest,
    SelectItem
} from '../../../models/index';

@Component({
    selector: 'step-registration',
    templateUrl: 'step-registration.component.html'
})

export class StepRegistrationComponent implements StepBaseComponent, OnInit {
    

    @Input() public returnItem: ReturnCalculationRequest;
    public reasonList: SelectItem[] = [];


    public ngOnInit(): void {
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

    public isCompleted(): boolean {
        if (this.returnItem === null) return false;
        if (this.returnItem.isFraude == undefined) return false;
        if (this.returnItem.isError == undefined) return false;
        if (this.returnItem.reason == undefined) return false;
        if (this.returnItem.reason == "") return false;
        if (this.returnItem.reason == "VALUES_REASON_OTHER") {
            if (this.returnItem.otherReason == undefined) return false;
            if (this.returnItem.otherReason == "") return false;
        }
        return true;
    }

    public cancel() {
        this.returnItem = new ReturnCalculationRequest();
    }

    public activate(): void {
    }
    public deactivate(): void {
    }

}