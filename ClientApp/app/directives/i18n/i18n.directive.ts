import { Directive, Input, ElementRef, OnInit } from '@angular/core';
import { I18nService } from './i18n.service';



@Directive({ selector: '[i18n]' })
export class I18nDirective implements OnInit {

    @Input() description : string;
    @Input('i18n') i18n : string;

    constructor(
        private el: ElementRef,
        private i18nService: I18nService) {
    }

    ngOnInit(): any {
        let innerHtml: string = this.el.nativeElement.innerHTML;
        if (innerHtml == undefined) return;

        let replacement: string = this.i18nService.getReplacementValue(innerHtml, this.i18n || this.description);
        
        this.setReplacementValue(replacement);
    }


    /** Private functions */
    public setReplacementValue = (value: string) => {
        let originalText: string = this.el.nativeElement.innerHTML;
        this.el.nativeElement.innerHTML = value;
    }

}