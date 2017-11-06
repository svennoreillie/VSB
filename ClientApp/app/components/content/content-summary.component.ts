import { Person } from "./../../models";
import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: 'content-summary',
    templateUrl: 'content-summary.component.html'
})

export class ContentSummaryComponent implements OnInit {
    @Input() public person: Person  = new Person();
    public vsbInfo: any;
    
    constructor() { }

    ngOnInit() { }
}