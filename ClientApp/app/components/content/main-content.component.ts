import { Component, Input } from "@angular/core";
import { ContentTab, GenderTypeModel, PersonModel } from '../../models';

@Component({
    selector: 'main-content',
    templateUrl: 'main-content.component.html'
})

export class MainContentComponent {
    //Enables the enum in angulare exressions
    public contentTab = ContentTab;
    public genderTypeModel = GenderTypeModel;

    public currentTab: ContentTab = ContentTab.Summary;
    @Input() public person: PersonModel  = new PersonModel();

    constructor() { }

    public setView(tab: ContentTab) {
        this.currentTab = tab;
    }

    public getActive(tab: ContentTab): boolean {
        return this.currentTab === tab;
    }
}
