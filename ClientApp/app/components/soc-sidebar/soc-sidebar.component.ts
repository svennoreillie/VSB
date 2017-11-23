import { Subscription } from "rxjs/Subscription";
import { Input, Output } from "@angular/core";
import { Component, EventEmitter, OnInit, OnDestroy } from "@angular/core";
import { PersonModel } from "./../../models/person";
import { Subject } from "rxjs/Subject";

@Component({
    selector: "soc-sidebar",
    templateUrl: "soc-sidebar.component.html",
    styleUrls: ["soc-sidebar.component.css"]
})

export class SocSidebarComponent implements OnInit, OnDestroy {
    
    public subscription: Subscription;
    public selectedPerson: PersonModel;
    @Input() public people: PersonModel[];
    @Input() public loading: boolean = false;
    @Output() public socDownload: EventEmitter<PersonModel[]> = new EventEmitter<PersonModel[]>();
    @Output() public socSelect: EventEmitter<PersonModel> = new EventEmitter<PersonModel>();
    @Output() public bottomScroll: EventEmitter<any> = new EventEmitter<any>();
    public scroll = new Subject<Event>();
    public formVisible: boolean = true;

    public ngOnInit(): void {
        this.subscription = this.scroll
            .map(event => event)
            .debounceTime(200)
            .subscribe(event => {
                let target: any = event.target;
                let position = target.scrollTop + target.clientHeight;
                let totalHeight = target.scrollHeight;
                if (position > (totalHeight * 0.85)) {
                    this.bottomScroll.emit();
                }
            });
    }

    public ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

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
