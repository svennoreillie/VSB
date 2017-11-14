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
    public peopleSubsricption: Subscription;

    public currentTab: ContentTab = ContentTab.Summary;
    public person: PersonModel | null;

    constructor(private peopleService: PeopleService) { }

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(p => this.personChanged(p));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        if (this.peopleService) this.peopleSubsricption.unsubscribe();
    }

    public setView(tab: ContentTab) {
        this.currentTab = tab;
    }

    public getActive(tab: ContentTab): boolean {
        return this.currentTab === tab;
    }

    private personChanged(person: PersonModel | null)  {
        this.person = person;
        if (person == null) return;
        this.peopleSubsricption = this.peopleService.get(person.siNumber)
                                                    .subscribe(response => this.person = response);
    }
}
