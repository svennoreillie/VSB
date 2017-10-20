import { Injectable } from "@angular/core";
import configJson from "../../../config.json";
import { Configuration } from "./configuration";

@Injectable()
export class ConfigService {

    private configuration: Configuration;

    constructor() {
        this.configuration = configJson;
    }

    public get config(): Configuration {
        return this.configuration;
    }
}
