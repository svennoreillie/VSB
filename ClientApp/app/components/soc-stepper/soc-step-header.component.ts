import { Component, Input } from '@angular/core';

@Component({
    selector: 'soc-step-header',
    templateUrl: 'soc-step-header.component.html'
})

export class SocStepHeaderComponent {
    @Input() public index: number;
    @Input() public label: string;
} 