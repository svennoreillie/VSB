import {
    Component,
    Input,
    ContentChildren,
    QueryList,
    Inject,
    forwardRef,
    Output,
    EventEmitter,
    SimpleChanges
} from '@angular/core';
import { SocStepComponent } from './index';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';


export class SocStepperChange {
    public oldIndex: number;
    public newIndex: number;
}


@Component({
    selector: 'soc-stepper',
    templateUrl: 'soc-stepper.component.html'
})
export class SocStepperComponent implements OnChanges {
    
    @ContentChildren(SocStepComponent) steps: QueryList < SocStepComponent > ;

    @Input() public current: number = 0;
    @Output() public currentChange: EventEmitter<number> = new EventEmitter<number>();
    @Output() public changed: EventEmitter<SocStepperChange> = new EventEmitter<SocStepperChange>();

    public previous() {
        this.goToStepIndex(this.current - 1);
    }

    public next() {
        this.goToStepIndex(this.current + 1);
    }

    public setStepNumber(step: number) {
        this.goToStepIndex(step - 1);
    }

    public setStep(step: SocStepComponent) {
        let oldIndex = this.current;
        let index: number = this.steps.toArray().indexOf(step);
        this.goToStepIndex(index);
    }

    public ngOnChanges(changes: SimpleChanges): void {
        let change = changes["current"];
        if (change.previousValue != change.currentValue) {
            this.goToStepIndex(change.currentValue);
        }    
    }

    private goToStepIndex(index: number) {
        if (index < 0) return;
        if (this.steps.length == 0) return;
        if (index > (this.steps.length + 1)) return;

        let possibleStep = this.steps.toArray()[index];
        if (possibleStep.valid) {
            this.changed.emit({ oldIndex: this.current, newIndex: index });
            this.current = index;
        }
    }

}