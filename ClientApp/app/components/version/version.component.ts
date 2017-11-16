import { Subscription } from "rxjs/Subscription";
import { Version } from "./../../models/admin/version";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { GeneralDataService } from "./../../services/api/general-data.service";

@Component({
    selector: "app-version",
    templateUrl: "./version.component.html"
})
export class VersionComponent implements OnInit, OnDestroy {
    private applicationVersionSupscription: Subscription;
    public applicationVersion: Version = new Version();

    constructor(private generalDataService: GeneralDataService) {}

    public ngOnInit(): void {
        this.applicationVersionSupscription = this.generalDataService.getVersion()
                                                                     .subscribe(data => this.applicationVersion = data);
    }

    public ngOnDestroy(): void {
        this.applicationVersionSupscription.unsubscribe();
    }
}
