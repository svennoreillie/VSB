import { Directive, HostListener, Input } from "@angular/core";
import { SocStepperComponent } from "./soc-stepper.component";

@Directive({
    selector: 'button[soc-step-next]',
    host: {'(click)': '_stepper.next()'}
})
export class SocStepNext  {
    constructor(public _stepper: SocStepperComponent) { }
}

@Directive({
    selector: 'button[soc-step-previous]',
    host: {'(click)': '_stepper.previous()'}
})
export class SocStepPrevious  {
    constructor(public _stepper: SocStepperComponent) { }
}


@Directive({
    selector: 'button[soc-step-go-to]'
})
export class SocStepGoTo {
    @Input('soc-step-go-to') step: number;

    constructor(public _stepper: SocStepperComponent) { }

    @HostListener('click', ['$event']) onClick($event: any) {
        this._stepper.setStepNumber(this.step);
    }
}
