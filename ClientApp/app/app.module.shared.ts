import { HttpErrorInterceptor } from "./services/interceptors/http-error.interceptor.service";
import {
    HttpClientModule, HTTP_INTERCEPTORS
} from "@angular/common/http";
import {
    MatDialogModule
} from "@angular/material/dialog";
import {
    PopoverModule
} from "ngx-popover"
import {
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatSelectModule,
    MatTooltipModule,
    MatSnackBarModule
} from "@angular/material";
// 3rd party
import * as angular from "./angular-barrel";
import { FileDropModule } from 'ngx-file-drop';

// Directives
import * as directives from "./directives";

// Services
import * as services from "./services";

// Components
import {
    AppComponent
} from "./components/app/app.component";
import * as content from "./components/content";
import {
    NavMenuComponent
} from "./components/navmenu/navmenu.component";
import * as search from "./components/search";
import {
    SocPanelComponent
} from "./components/soc-panel/soc-panel.component";
import {
    SocSidebarComponent
} from "./components/soc-sidebar/soc-sidebar.component";
import {
    VersionComponent
} from "./components/version/version.component";

// rxjs
import "rxjs/add/operator/catch";
import "rxjs/add/operator/debounceTime";
import "rxjs/add/operator/distinctUntilChanged";
import "rxjs/add/operator/map";
import "rxjs/add/operator/share";
import "rxjs/add/operator/shareReplay";
import "rxjs/add/operator/toPromise";
import "rxjs/add/observable/forkJoin";
import { HttpCacheInterceptor } from "./services";

@angular.NgModule({
    declarations: [
        AppComponent,
        // Components
        VersionComponent,
        NavMenuComponent,
        SocSidebarComponent,
        SocPanelComponent,
        search.SearchByNameComponent,
        search.SearchByMembernrComponent,
        search.SearchByInszComponent,
        search.SearchByStateComponent,
        content.MainContentComponent,
        content.ContentSummaryComponent,
        content.ContentBobComponent,
        content.ContentZvzComponent,
        content.ContentThabComponent,
        content.ContentAttachmentComponent,
        // Directives
        directives.TranslateDirective,
        directives.TranslatePipe,
        directives.SafeHtmlPipe,
    ],
    providers: [
        // Services
        directives.TranslateService,
        services.GeneralDataService,
        services.PeopleService,
        services.BobService,
        services.ThabService,
        services.ZvzService,
        services.UrlService,
        services.VSBService,
        services.ConfigService,
        services.NotificationService,
        services.CacheService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpErrorInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpCacheInterceptor,
            multi: true,
        },
    ],
    imports: [
        //material
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        MatFormFieldModule,
        MatButtonModule,
        MatCheckboxModule,
        MatRadioModule,
        MatCardModule,
        MatProgressSpinnerModule,
        MatProgressBarModule,
        MatSelectModule,
        MatTooltipModule,
        MatDialogModule,
        MatSnackBarModule,
        //3rd pary
        PopoverModule,
        FileDropModule,
        // Modules
        HttpClientModule,
        angular.CommonModule,
        angular.FormsModule,
        angular.ReactiveFormsModule,
        angular.HttpModule,
        // angular.HttpModule,
        angular.RouterModule.forRoot([{
                path: "",
                redirectTo: "byname",
                pathMatch: "full"
            },
            {
                path: "byname",
                component: search.SearchByNameComponent
            },
            {
                path: "byinsz",
                component: search.SearchByInszComponent
            },
            {
                path: "bymembernr",
                component: search.SearchByMembernrComponent
            },
            {
                path: "bystate",
                component: search.SearchByStateComponent
            },
            {
                path: "**",
                redirectTo: "byname"
            },
        ]),
    ],
})
export class AppModuleShared {}