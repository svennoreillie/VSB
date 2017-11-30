import {
    NotificationService
} from "./../../services/notification/notification.service";
import {
    GeneralDataService
} from "./../../services/api/general-data.service";
import {
    VSBService
} from "./../../services/api/vsb.service";
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
    Component,
    OnInit,
    OnDestroy,
    ViewChild,
    Inject,
    PLATFORM_ID
} from "@angular/core";
import {
    PeopleService
} from "../../services/index";
import {
    Attachment
} from "../../models/index";
import {
    UploadEvent
} from "ngx-file-drop";
import {
    isPlatformBrowser
} from '@angular/common';

@Component({
    selector: 'content-attachment',
    templateUrl: 'content-attachment.component.html'
})
export class ContentAttachmentComponent implements OnInit, OnDestroy {

    //Properties
    private userSub: Subscription;
    private activePersonSubscription: Subscription;

    public attachments: Observable < Attachment[] > ;
    public person: PersonModel | null;
    public username: string;
    public isBrowserMode: boolean = false;


    //Lifecycle hooks
    constructor(private peopleService: PeopleService,
        private vsbService: VSBService,
        private generalService: GeneralDataService,
        private notificationService: NotificationService,
        @Inject(PLATFORM_ID) private platformId: Object) {}

    public ngOnInit(): void {
        this.userSub = this.generalService.getUser().subscribe(data => this.username = data.user);

        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => {
            this.person = person;
            if (this.person != null) {
                this.attachments = this.vsbService.getAttachments(this.person.siNumber);
            }
        });

        if (isPlatformBrowser(this.platformId)) {
            this.isBrowserMode = true;
        }
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.userSub.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }


    public dropped(event: UploadEvent) {
        for (var file of event.files) {
            file.fileEntry.file((info: any) => {
                const formData = new FormData();
                formData.append("file", info);
                this.vsbService.postAttachment(this.person.siNumber, this.username, formData)
                    .subscribe(res => {
                        console.log(res);
                        this.notificationService.showInfo("File uploaded");
                    });
            });
        }
    }

    public download(att: Attachment) {
        console.log("download");
    }

    public remove(att: Attachment) {
        console.log("remove");
    }


}