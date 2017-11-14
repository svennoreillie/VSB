import { HttpClientModule } from "@angular/common/http";
// 3rd party
import * as angular from "./angular-barrel";
import {
    MatNativeDateModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatRadioModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatProgressBarModule
} from "@angular/material";

// Directives
import * as directives from "./directives";

// Services
import * as services from "./services";

// Components
import { AppComponent } from "./components/app/app.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { SocSidebarComponent } from "./components/soc-sidebar/soc-sidebar.component";
import { SocPanelComponent } from "./components/soc-panel/soc-panel.component";
import * as search from "./components/search";
import * as content from "./components/content";

//rxjs
import "rxjs/add/operator/catch";
import "rxjs/add/operator/map";
import "rxjs/add/operator/shareReplay";
import "rxjs/add/operator/share";
import "rxjs/add/operator/toPromise";


@angular.NgModule({
    declarations: [
        AppComponent,
        // Components
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
        // Directives
        directives.TranslateDirective,
        directives.TranslatePipe,
    ],
    providers: [
        // Services
        directives.TranslateService,
        services.GeneralDataService,
        services.PeopleService,
        services.BobService,
        services.UrlService,
        services.ConfigService,
    ],
    imports: [
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
        // Modules
        HttpClientModule,
        angular.CommonModule,
        angular.FormsModule,
        angular.ReactiveFormsModule,
        //angular.HttpModule,
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