import { Person } from "./../../models";
import { Component, OnInit, Input } from "@angular/core";

@Component({
    selector: 'content-bob',
    templateUrl: 'content-bob.component.html'
})

export class ContentBobComponent implements OnInit {
    @Input() public person: Person  = new Person();
    public vsbInfo: any;
    
    constructor() { }

    ngOnInit() { }
}