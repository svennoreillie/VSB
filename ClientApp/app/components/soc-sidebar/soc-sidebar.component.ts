import { Input, Output } from "@angular/core";
import { Component, EventEmitter, OnInit } from "@angular/core";
import { PersonModel } from "./../../models/person";

@Component({
    selector: "soc-sidebar",
    templateUrl: "soc-sidebar.component.html"
})

export class SocSidebarComponent implements OnInit {

    public selectedPerson: PersonModel;
    @Input() public people: PersonModel[];
    @Output() public download: EventEmitter<PersonModel[]> = new EventEmitter<PersonModel[]>();
    @Output() public select: EventEmitter<PersonModel> = new EventEmitter<PersonModel>();

    private formVisible: boolean = true;

    constructor() { }

    public ngOnInit() {

    }

    public downloadPeople() {
        this.download.emit(this.people);
    }

    public selectPerson(p: PersonModel ) {
        this.selectedPerson = p;
        this.select.emit(this.selectedPerson);
    }

    public collapseForm = () => {
        this.formVisible = this.formVisible;
    }
}
