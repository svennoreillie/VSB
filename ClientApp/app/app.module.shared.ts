// 3rd party
import * as angular from "./angular-barrel";

// Directives
import * as directives from "./directives";

// Services
import * as services from "./services";

// Components
import { AppComponent } from "./components/app/app.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { SearchByNameComponent } from "./components/search/searchbyname/search-by-name.component";
import { SocSidebarComponent } from "./components/soc-sidebar/soc-sidebar.component";

@angular.NgModule({
    declarations: [
        AppComponent,
        // Components
        SearchByNameComponent,
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
        // Modules
        angular.CommonModule,
        angular.FormsModule,
        angular.HttpModule,
        angular.RouterModule.forRoot([{
                path: "",
                redirectTo: "byname",
                pathMatch: "full"
            },
            {
                path: "byname",
                component: SearchByNameComponent
            },
            {
                path: "**",
                redirectTo: "byname"
            },
        ]),
    ],
})
export class AppModuleShared {}
