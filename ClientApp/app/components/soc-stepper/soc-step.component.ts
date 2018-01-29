import { Component, Input, Inject, forwardRef, ViewChild, TemplateRef } from "@angular/core";
import { SocStepperComponent } from "./index";

@Component({
    selector: 'soc-step',
    templateUrl: 'soc-step.component.html'
})
export class SocStepComponent {

    @Input() public title: string;
    @Input() public activeTitle: string;
    @Input() public valid: boolean;

    /** Template for step content. */
    @ViewChild(TemplateRef) content: TemplateRef<any>;

    constructor(@Inject(forwardRef(() => SocStepperComponent)) private _stepper: SocStepperComponent) { }

    public select() {
        this._stepper.setStep(this);
    }
}
