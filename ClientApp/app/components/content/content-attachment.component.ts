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
    ViewChild
} from "@angular/core";
import {
    PeopleService
} from "../../services/index";
import {
    Attachment
} from "../../models/index";

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


    //Lifecycle hooks
    constructor(private peopleService: PeopleService,
        private vsbService: VSBService,
        private generalService: GeneralDataService,
        private notificationService: NotificationService) {}

    public ngOnInit(): void {
        this.userSub = this.generalService.getUser().subscribe(data => this.username = data.user);

        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => {
            this.person = person;
            if (this.person != null) {
                this.attachments = this.vsbService.getAttachments(this.person.siNumber);
            }
        });
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.userSub.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }

    @ViewChild('fileInput') fileInput: any;

    public upload() {
        if (this.person != null) {
            let fileBrowser = this.fileInput.nativeElement;
            if (fileBrowser.files && fileBrowser.files[0]) {
                const formData = new FormData();
                formData.append("file", fileBrowser.files[0]);
                this.vsbService.postAttachment(this.person.siNumber, this.username, formData)
                    .subscribe(res => {
                        console.log(res);
                        this.notificationService.showInfo("File uploaded");
                    });
            }
        }
    }

    public download(att: Attachment) {
        console.log("download");
    }

    public remove(att: Attachment) {
        console.log("remove");
    }

    // public dragFileAccepted(acceptedFile: Ng2FileDropAcceptedFile) {

    //     let fileReader = new FileReader();
    //     fileReader.onload = () => {
    //         if (this.person == null) return;

    //         const formData = new FormData();
    //         formData.append("image", fileReader.result);
    //         this.vsbService.postAttachment(this.person.siNumber, this.username, formData)
    //             .subscribe(res => {
    //                 console.log(res);
    //                 this.notificationService.showInfo("File uploaded");
    //             });
    //     };

    //     // Read in the file
    //     fileReader.readAsDataURL(acceptedFile.file);
    // }

}