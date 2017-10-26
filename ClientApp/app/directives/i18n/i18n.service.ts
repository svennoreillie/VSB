import {
    Injectable
} from "@angular/core";
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
        originalText = originalText.replace(/\n/g, " ");


        let regexPieces = /^(<.*>)/g.exec(originalText);
        if (regexPieces != null) {
            regexPieces.forEach((regexPiece) => {
                originalText = originalText.replace(regexPiece, " ");
            });
        }

        originalText = originalText.trim();
        if (originalText.length == 0) {
            throw new Error("I18N translation, only found html. consider using the pipe version => " + originalValue);
        }

        return originalText;
    }

    public getTransformedText = (originalValue: string): string => {
        const text = this.getOriginalText(originalValue);
        const transformedText = text.replace(/ /g, "_");
        return transformedText.toUpperCase();
    }
}
