import { Input, Output } from "@angular/core";
import { Component, EventEmitter, OnInit } from "@angular/core";
import { Person } from "./../../models/person";

@Component({
    selector: "soc-sidebar",
    templateUrl: "soc-sidebar.component.html"
})

export class SocSidebarComponent implements OnInit {

    public selectedPerson: Person;
    @Input() public people: Person[];
    @Output() public download: EventEmitter<Person[]> = new EventEmitter<Person[]>();
    @Output() public select: EventEmitter<Person> = new EventEmitter<Person>();

    private formVisible: boolean = true;

    constructor() { }

    public ngOnInit() {

    }

    public downloadPeople() {
        this.download.emit(this.people);
    }

    public selectPerson(p: Person ) {
        this.selectedPerson = p;
        this.select.emit(this.selectedPerson);
    }

    public collapseForm = () => {
        this.formVisible = this.formVisible;
    }
}
