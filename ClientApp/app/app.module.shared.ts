import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { RouterModule } from "@angular/router";

import { AppComponent } from "./components/app/app.component";
import { CounterComponent } from "./components/counter/counter.component";
import { FetchDataComponent } from "./components/fetchdata/fetchdata.component";
import { HomeComponent } from "./components/home/home.component";
import { NavMenuComponent } from "./components/navmenu/navmenu.component";

// Components
import { SearchByNameComponent } from "./components/search/searchbyname/search-by-name.component";
import { SocSidebarComponent } from "./components/soc-sidebar/soc-sidebar.component";

// Directives
import * as directives from "./directives";

// Services
import * as services from "./services";

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,

        SearchByNameComponent,
        SocSidebarComponent,

        directives.I18nDirective,
        directives.I18nPipe,
    ],
    providers: [
        directives.I18nService,
        services.GeneralDataService,
        services.SearchPersonService,
        services.UrlService,
        services.ConfigService,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            { path: "byname", component: SearchByNameComponent },
            { path: "counter", component: CounterComponent },
            { path: "fetch-data", component: FetchDataComponent },
            { path: "**", redirectTo: "home" },
        ]),
    ],
})
export class AppModuleShared {
}
