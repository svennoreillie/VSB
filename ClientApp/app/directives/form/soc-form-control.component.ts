import { Component, Input, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import { I18nService } from "./../i18n/i18n.service";
import { SocFormValidator } from "./soc-form-validator";

@Component({
    selector: "soc-form-control",
    templateUrl: "soc-form-control.component.html"
})

export class SocFormControlComponent implements OnInit {

    @Input() public type: string;
    @Input() public property: string;
    @Input() public placeholder: string;
    @Input() public validators: SocFormValidator[];

    public control = new FormControl();

    constructor(private i18nService: I18nService) { }

    public ngOnInit() {
        if (this.validators != null && this.validators.length) {
            this.control.setValidators(this.validators.map((v) => v.validator));
        }
    }

    public getErrorMessage = (): string => {
        this.validators.forEach((validator) => {
            if (this.control.hasError(validator.validator.name)) {
                const description = "ErrorMsg=>" + this.type + "|" + this.placeholder;
                return this.i18nService.getReplacementValue(validator.errorMessage, description);
            }
        });

        return "";
    }
}
