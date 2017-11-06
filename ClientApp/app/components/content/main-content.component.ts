import { Person } from "./../../models/person";
import { Component, Input } from "@angular/core";
import { ContentTab, GenderTypeModel } from '../../models';

@Component({
    selector: 'main-content',
    templateUrl: 'main-content.component.html'
})

export class MainContentComponent {
    //Enables the enum in angulare exressions
    public contentTab = ContentTab;
    public genderTypeModel = GenderTypeModel;

    public currentTab: ContentTab = ContentTab.Summary;
    @Input() public person: Person  = new Person();

    constructor() { }

    public setView(tab: ContentTab) {
        this.currentTab = tab;
    }

    public getActive(tab: ContentTab): boolean {
        return this.currentTab === tab;
    }
}
