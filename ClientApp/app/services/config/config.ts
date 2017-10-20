import { Injectable, OnInit } from "@angular/core";
import configuration from "../../../config.json";
import { Configuration } from "./configuration";

@Injectable()
export class ConfigService implements OnInit {

    private configuration: Configuration;

    public ngOnInit(): void {
        this.configuration = configuration;
    }

    public get config(): Configuration {
        return this.configuration;
    }
}
