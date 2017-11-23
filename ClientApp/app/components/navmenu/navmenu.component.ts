import { Observable } from "rxjs/Observable";
import { GeneralDataService } from "./../../services/api/general-data.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "nav-menu",
    templateUrl: "./navmenu.component.html",
})
export class NavMenuComponent implements OnInit, OnDestroy {
    
    private userSub: Subscription;
    public userName: string;

    constructor(private dataService: GeneralDataService) {

    }

    public ngOnInit(): void {
        this.userSub = this.dataService.getUser().subscribe(data => this.userName = data.user);
    }

    public ngOnDestroy(): void {
        this.userSub.unsubscribe();
    }
}
