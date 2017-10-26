// 3rd party
import * as angular from "./angular-barrel";

// Directives
import * as directives from "./directives";

// Services
import * as services from "./services";

// Components
import { MatNativeDateModule, MatDatepickerModule, MatFormFieldModule, MatInputModule, MatButtonModule } from "@angular/material";
import { AppComponent } from "./components/app/app.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import * as search from "./components/search";
import { SocSidebarComponent } from "./components/soc-sidebar/soc-sidebar.component";

@angular.NgModule({
    declarations: [
        AppComponent,
        // Components
        search.SearchByNameComponent,
        search.SearchByMembernrComponent,
        search.SearchByInszComponent,
        search.SearchByStateComponent,
        SocSidebarComponent,
        NavMenuComponent,
        // Directives
        directives.I18nDirective,
        directives.I18nPipe,
    ],
    providers: [
        // Services
        directives.I18nService,
        services.GeneralDataService,
        services.PeopleService,
        services.UrlService,
        services.ConfigService,
    ],
    imports: [
        MatDatepickerModule,
        MatNativeDateModule,
        MatInputModule,
        MatFormFieldModule,
        MatButtonModule,
        // Modules
        angular.CommonModule,
        angular.FormsModule,
        angular.ReactiveFormsModule,
        angular.HttpModule,
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
