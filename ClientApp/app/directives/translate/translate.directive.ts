import { Directive, Input, ElementRef, OnInit } from '@angular/core';
import { TranslateService } from './translate.service';

@Directive({ selector: '[translate]' })
export class TranslateDirective implements OnInit {
    

    @Input() description : string;
    @Input('translate') i18n : string;

    constructor(
        private el: ElementRef,
        private translateService: TranslateService) {
    }

    ngOnInit(): any {
        let innerHtml: string = this.el.nativeElement.innerHTML;
        if (innerHtml == undefined) return;

        let replacement: string = this.translateService.getReplacementValue(innerHtml, this.i18n || this.description);
        
        this.setReplacementValue(replacement);
    }

    /** Private functions */
    public setReplacementValue = (value: string) => {
        this.el.nativeElement.innerHTML = value;
    }

}