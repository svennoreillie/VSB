import { Directive, Input, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { TranslateService } from './translate.service';

@Directive({ selector: '[translate]' })
export class TranslateDirective implements OnInit {
    

    @Input() description : string;
    @Input('translate') i18n : string;

    constructor(private element: ElementRef, private renderer: Renderer2, private translateService: TranslateService) {
    }

    ngOnInit(): any {
        let innerHtml: string = this.element.nativeElement.innerHTML;
        if (innerHtml == undefined) return;

        let replacement: string = this.translateService.getReplacementValue(innerHtml, this.i18n || this.description);
        
        this.setReplacementValue(replacement);
    }

    /** Private functions */
    public setReplacementValue = (value: string) => {
        this.renderer.setProperty(this.element.nativeElement, 'innerHTML', value);
    }

}