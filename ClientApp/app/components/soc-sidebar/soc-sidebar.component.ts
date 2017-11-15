import { Input, Output } from "@angular/core";
import { Component, EventEmitter } from "@angular/core";
import { PersonModel } from "./../../models/person";

@Component({
    selector: "soc-sidebar",
    templateUrl: "soc-sidebar.component.html"
})

export class SocSidebarComponent {

    public selectedPerson: PersonModel;
    @Input() public people: PersonModel[];
    @Input() public loading: boolean = false;
    @Output() public socDownload: EventEmitter<PersonModel[]> = new EventEmitter<PersonModel[]>();
    @Output() public socSelect: EventEmitter<PersonModel> = new EventEmitter<PersonModel>();

    private formVisible: boolean = true;

    public downloadPeople() {
        this.socDownload.emit(this.people);
    }

    public selectPerson(person: PersonModel ) {
        this.selectedPerson = person;
        this.socSelect.emit(this.selectedPerson);
    }

    public collapseForm = () => {
        this.formVisible = !this.formVisible;
    }
}
