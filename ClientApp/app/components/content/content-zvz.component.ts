import { ZVZWarranty, ZVZLetter, ZVZPayment } from "./../../models";
import { PersonModel } from "./../../models/person";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { PeopleService } from "./../../services/api/people.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { ZvzService } from "../../services/api/zvz.service";

@Component({
    selector: 'content-zvz',
    templateUrl: 'content-zvz.component.html'
})
export class ContentZvzComponent implements OnInit, OnDestroy {
          
    //Properties
    public activePersonSubscription: Subscription;
    public selectedWarranty: ZVZWarranty | null;

    public warrantiesSubscription: Subscription;
    public warrantiesLoading: boolean;
    public warranties: ZVZWarranty[];

    public person: PersonModel | null;
    public letters: Observable<ZVZLetter[]>;
    public payments: Observable<ZVZPayment[]>;


    //Lifecycle hooks
    constructor(private zvzService: ZvzService, private peopleService: PeopleService) {}

    public ngOnInit(): void {
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => this.personChanged(person));
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        if (this.warrantiesSubscription) this.warrantiesSubscription.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    public showPayments(): boolean {
        return false;
    }

    public selectWarranty(warranty: ZVZWarranty) {
        this.selectedWarranty = warranty;
        if (this.person != null) {
            this.payments = this.zvzService.getSpecificPayments(this.person.siNumber, warranty.RequestDate);        
        }
    }

   
    //Private Methods
    private personChanged(person: PersonModel | null) {
        this.person = person;
        this.loadData();
    }

    private loadData() {
        if (this.person == null) return;

        this.letters = this.zvzService.getLetters(this.person);

        this.warrantiesLoading = true;
        this.warrantiesSubscription = this.zvzService.getWarranties(this.person.siNumber)
                                                     .subscribe(data => {
                                                         this.warranties = data;
                                                         if (data.length == 0) this.selectedWarranty = data[0];
                                                         else this.selectedWarranty = null;
                                                     }, 
                                                     (error) => console.error(error), 
                                                     () => this.warrantiesLoading = false);
    }

}
