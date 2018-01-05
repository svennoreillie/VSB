import {
    Component,
    Input,
    ContentChildren,
    QueryList,
    Inject,
    forwardRef,
} from '@angular/core';
import { SocStepComponent } from './index';


@Component({
    selector: 'soc-stepper',
    templateUrl: 'soc-stepper.component.html'
})
export class SocStepperComponent {

    @ContentChildren(SocStepComponent) steps: QueryList < SocStepComponent > ;

    public selectedIndex: number = 0;

    public previous() {

    }

    public next() {
        
    }

}