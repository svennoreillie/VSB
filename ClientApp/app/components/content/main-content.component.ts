import { ActiveContentPageService } from "./../../services/activecontent/active-content-page.service";
import { Subscription } from "rxjs/Subscription";
import { PeopleService } from "./../../services/api/people.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { ContentTab, GenderTypeModel, PersonModel } from '../../models';

@Component({
    selector: 'main-content',
    templateUrl: 'main-content.component.html'
})

export class MainContentComponent implements OnInit, OnDestroy {
    //Enables the enum in angulare exressions
    public contentTab = ContentTab;
    public genderTypeModel = GenderTypeModel;

    public activePersonSubscription: Subscription;
    public updatePersonSubscription: Subscription;
    //public peopleSubsricption: Subscription;

    public currentTab: ContentTab = ContentTab.Summary;
    public person: PersonModel | null;

    constructor(private peopleService: PeopleService, private activeContentService: ActiveContentPageService) { }

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(p => this.person = p);
        this.updatePersonSubscription = this.peopleService.activePersonFullDetails.subscribe(p => this.person = p);
        this.activeContentService.activePage = ContentTab.Summary;
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.updatePersonSubscription.unsubscribe();
    }

    public setView(tab: ContentTab) {
        this.currentTab = tab;
        this.activeContentService.activePage = tab;
    }

    public getActive(tab: ContentTab): boolean {
        return this.activeContentService.activePage === tab;
    }
}
