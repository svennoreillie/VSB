import {
    TranslateService
} from "./../../directives/translate/translate.service";
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
import { MatSnackBar } from '@angular/material';
import { ChangeDetectorRef } from '@angular/core';

@Component({
    selector: 'content-attachment',
    templateUrl: 'content-attachment.component.html'
})
export class ContentAttachmentComponent implements OnInit, OnDestroy  {

    //Properties
    private userSub: Subscription;
    private postSub: Subscription;
    private attachmentsSub: Subscription;
    private activePersonSubscription: Subscription;
    private removeSub: Subscription;
    private downloadSub: Subscription;
    private filePostObservables: Observable<Attachment>[];
    private attachmentsLoading: boolean;

    public attachments: Attachment[];
    public uploadLoading: boolean;
    public person: PersonModel | null;
    public username: string;
    public isBrowserMode: boolean = false;


    //Lifecycle hooks
    constructor(private peopleService: PeopleService,
        private vsbService: VSBService,
        private generalService: GeneralDataService,
        private notificationService: NotificationService,
        private translationService: TranslateService,
        private snackBar: MatSnackBar,
        private changeDetector: ChangeDetectorRef,
        @Inject(PLATFORM_ID) private platformId: Object) {
    }


    public ngOnInit(): void {
        this.userSub = this.generalService.getUser().subscribe(data => this.username = data.user);

        this.activePersonSubscription = this.peopleService.activePerson.subscribe(person => {
            this.person = person;

            this.getAttachments();
        });

        if (isPlatformBrowser(this.platformId)) {
            this.isBrowserMode = true;
        }
    }

    public ngOnDestroy(): void {
        this.activePersonSubscription.unsubscribe();
        this.userSub.unsubscribe();
        if (this.attachmentsSub) this.attachmentsSub.unsubscribe();
        if (this.removeSub) this.removeSub.unsubscribe();
        if (this.downloadSub) this.downloadSub.unsubscribe();
        if (this.postSub) this.postSub.unsubscribe();
    }



    //Template methods
    public hasData(): boolean {
        return this.person !== undefined && this.person !== null;
    }


    public dropped(event: UploadEvent) {
        this.filePostObservables = [];
        for (var file of event.files) {
            file.fileEntry.file((info: any) => {
                const formData = new FormData();
                formData.append("file", info);
                this.filePostObservables.push(this.vsbService.postAttachment(this.person.siNumber, formData));
                if (this.filePostObservables.length == event.files.length) this.postFiles();
            });
        }
    }

    private postFiles() {
        if (this.filePostObservables.length > 0) {
            this.uploadLoading = true;
            this.changeDetector.detectChanges();
            Observable.forkJoin(this.filePostObservables).subscribe(() => {
                this.uploadLoading = false;
                this.getAttachments(false);
                this.filePostObservables = [];
            });
        }
    }

    private getAttachments(loading ? : boolean) {        
        if (this.person != null) {
            if (loading) this.attachmentsLoading = true;
            this.attachmentsSub = this.vsbService.getAttachments(this.person.siNumber, true)
                .subscribe(data => {
                    this.attachments = data
                    this.changeDetector.detectChanges();
                },
                    err => {},
                    () => this.attachmentsLoading = false);
        }
    }

    public download(att: Attachment) {
        if (this.person !== null) {
            this.downloadSub = this.vsbService.downloadAttachment(this.person.siNumber, att.filename)
                .subscribe(blob => {
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    if (this.person != null) link.download = att.filename;
                    link.click();
                }, error => console.error(error));
        }
    }

    public remove(att: Attachment) {
        if (this.person != null) {
            var snackbar = this.snackBar.open(this.translationService.getReplacementValue("Delete attachment question"), this.translationService.getReplacementValue("Delete"), {
                duration: 3000,
                extraClasses: ['soc-warning']
              });
              snackbar.onAction().subscribe(() =>  {
                this.removeSub = this.vsbService.removeAttachment(this.person.siNumber, att.filename)
                .subscribe(res => {
                        console.log(res);
                        this.notificationService.showInfo(this.translationService.getReplacementValue("File deleted"));
                    },
                    err => {},
                    () => this.getAttachments(false));
              });
            
        }
    }

}
