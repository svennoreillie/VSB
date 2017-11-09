import { Pipe, PipeTransform } from '@angular/core';
import { TranslateService } from './translate.service';

@Pipe({
    name: 'translate'
})

export class TranslatePipe implements PipeTransform {
    
    constructor(private translateService: TranslateService) {
    }

    transform(value: string, description: string): any {
        let replacement: string = this.translateService.getReplacementValue(value, description);
        
        return replacement;
    }
}