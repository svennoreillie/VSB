import {
    Subscription
} from "rxjs/Subscription";
import {
    Component,
    OnDestroy,
    OnInit
} from "@angular/core";
import {
    NotificationService,
    PeopleService
} from "../../services/index";
import {
    TranslateService
} from "../../directives/index";
import {
    MatSnackBar
} from "@angular/material";
import {
    PersonModel,
    ReturnReason
} from "../../models/index";


@Component({
    selector: 'content-returns',
    templateUrl: 'content-returns.component.html'
})
export class ContentReturnsComponent implements OnInit, OnDestroy {


    //Properties
    private activePersonSubscription: Subscription;
    private person: PersonModel | null;

    public returnItem: any = {};
    public reasonList: ReturnReason[] = []
    public step1Valid: boolean = true;
    public step2Valid: boolean = true;

    //Lifecycle hooks
    constructor(
        private peopleService: PeopleService,
        private notificationService: NotificationService,
        private translationService: TranslateService,
        private snackBar: MatSnackBar) {}


    public ngOnInit(): void {
        this.fillReturnList();
        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => {
            this.person = person;
        });
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
    }


    private fillReturnList() {
        this.reasonList = [];
        for (var index = 0; index < 4; index++) {
            this.reasonList.push({
                index: index,
                value: `VALUES_REASON_${index}`,
                displayValue: `ReturnReason${index}`
            });
        }
        this.reasonList.push({
            index: 4,
            value: "VALUES_REASON_OTHER",
            displayValue: "ReturnReasonOther"
        });
    }


    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

}