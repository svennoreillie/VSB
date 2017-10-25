import { Component, OnInit } from "@angular/core";
import { FormControl } from "@angular/forms";
import { GeneralDataService } from "./../../services/api/general-data.service";

@Component({
    selector: "app",
    templateUrl: "./app.component.html"
})
export class AppComponent implements OnInit {

    public applicationVersion: any;

    constructor(private generalDataService: GeneralDataService) {}

    public ngOnInit(): void {
        this.applicationVersion = this.generalDataService.getVersion();
    }

}
