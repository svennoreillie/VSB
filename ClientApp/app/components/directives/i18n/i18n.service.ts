import { Injectable } from '@angular/core';
import nl from '../../../../translations/nl.json'

@Injectable()
export class I18nService {

    constructor() { }

    public getReplacementValue = (originalValue: string, description: string): string => {
        let original: string = this.getOriginalText(originalValue);
        let key: string = this.getTransformedText(originalValue);

        let translation = nl[key];
        if (!translation) {
            let newTranslation = {
                originalText: original,
                description: description,
                value: ""
            };
            let jsonExampleTranslation = '"' + key + '" : ' + JSON.stringify(newTranslation)
            console.warn('Translation not found, add the following json block to the translation files \n\n' + jsonExampleTranslation);
            return key;
        }
        if (!translation.value) return translation.originalText;
        return translation.value as string;
    }

    public getOriginalText = (originalValue: string): string => {
        let originalText: string = originalValue;
        originalText = originalText.trim();

        //Check for html elements and cut them
        let htmlStartCharPosition = originalText.indexOf('<');
        if (htmlStartCharPosition === 0) {
            //starts with html element right away => ignore this
            throw Error("Element contains html instead of text");
        } else if (htmlStartCharPosition > 0) {
            //element found => substring text part
            originalText = originalText.substring(0, htmlStartCharPosition);
        }

        return originalText;
    }

    public getTransformedText = (originalValue: string): string => {
        let text = this.getOriginalText(originalValue);
        let transformedText = text.replace(/ /g, "_");
        return transformedText.toUpperCase();
    }
}