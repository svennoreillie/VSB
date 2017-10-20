import { Injectable } from "@angular/core";
import nl from "../../../translations/nl.json";

@Injectable()
export class I18nService {

    public getReplacementValue = (originalValue: string, description: string): string => {
        const original: string = this.getOriginalText(originalValue);
        const key: string = this.getTransformedText(originalValue);

        const translation = nl[key];
        if (!translation) {
            const newTranslation = {
                originalText: original,
                description,
                value: "",
            };

            const translationWarning = "Translation not found, add the following json block to the translation files ";
            const jsonExampleTranslation = '"' + key + '" : ' + JSON.stringify(newTranslation) + ",";
            console.warn(translationWarning + "\n\n" + jsonExampleTranslation);
            
            return key;
        }
        if (!translation.value) return translation.originalText;
        return translation.value as string;
    }

    public getOriginalText = (originalValue: string): string => {
        let originalText: string = originalValue;
        originalText = originalText.trim();

        // Check for html elements and cut them
        const htmlStartCharPosition = originalText.indexOf("<");
        if (htmlStartCharPosition === 0) {
            // starts with html element right away => ignore this
            throw Error("Element contains html instead of text");
        } else if (htmlStartCharPosition > 0) {
            // element found => substring text part
            originalText = originalText.substring(0, htmlStartCharPosition);
        }

        return originalText;
    }

    public getTransformedText = (originalValue: string): string => {
        const text = this.getOriginalText(originalValue);
        const transformedText = text.replace(/ /g, "_");
        return transformedText.toUpperCase();
    }
}
