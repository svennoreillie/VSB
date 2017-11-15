import {
    ZVZWarranty,
    ZVZLetter,
    ZVZPayment
} from "./../../models";
import {
    PersonModel
} from "./../../models/person";
import {
    Observable
} from "rxjs/Observable";
import {
    Subscription
} from "rxjs/Subscription";
import {
    PeopleService
} from "./../../services/api/people.service";
import {
    Component,
    OnInit,
    OnDestroy
} from "@angular/core";
import {
    ZvzService
} from "../../services/api/zvz.service";

@Component({
    selector: 'content-zvz',
    templateUrl: 'content-zvz.component.html'
})
export class ContentZvzComponent implements OnInit, OnDestroy {

    //Properties
    public activePersonSubscription: Subscription;
    public activePersonFullDetailSubscription: Subscription;
    public selectedWarranty: ZVZWarranty | null;

    public warrantiesSubscription: Subscription;
    public warrantiesLoading: boolean;
    public warranties: ZVZWarranty[];

    public person: PersonModel | null;
    public letters: Observable < ZVZLetter[] > ;
    public payments: Observable < ZVZPayment[] > ;


    //Lifecycle hooks
    constructor(private zvzService: ZvzService, private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson
            .subscribe(person => this.personChanged(person));
        this.activePersonFullDetailSubscription = this.peopleService.activePersonFullDetails
            .subscribe(person => this.fullDetailsLoaded(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.activePersonFullDetailSubscription.unsubscribe();
        if (this.warrantiesSubscription) this.warrantiesSubscription.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public showPayments(): boolean {
        return this.selectedWarranty !== null && this.selectedWarranty !== undefined;
    }

    public selectWarranty(warranty: ZVZWarranty | null) {
        this.selectedWarranty = warranty;
        if (this.person != null && warranty != null) {
            this.payments = this.zvzService.getSpecificPayments(this.person.siNumber, warranty.requestDate).share();
        }
    }


    //Private Methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
        this.loadWarranties();
    }

    private fullDetailsLoaded(person: PersonModel | null) {
        this.person = person;
        if (this.person == null) return;
        this.letters = this.zvzService.getLetters(this.person).share();
    }

    private loadWarranties() {
        if (this.person == null) return;
        this.warrantiesLoading = true;
        this.warrantiesSubscription = this.zvzService.getWarranties(this.person.siNumber)
            .share()
            .subscribe(data => {
                    this.warranties = data;
                    if (data.length > 0) this.selectWarranty(data[0]);
                    else this.selectWarranty(null);
                },
                (error) => console.error(error),
                () => this.warrantiesLoading = false);
    }

}