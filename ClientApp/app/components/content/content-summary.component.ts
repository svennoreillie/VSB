import { PersonModel } from "./../../models";
import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: 'content-summary',
    templateUrl: 'content-summary.component.html'
})

export class ContentSummaryComponent implements OnInit {
    @Input() public person: PersonModel  = new PersonModel();
    public vsbInfo: any;
    
    constructor() { }

    ngOnInit() { }
}