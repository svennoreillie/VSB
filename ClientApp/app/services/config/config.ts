import { Configuration } from './configuration';
import { Injectable, OnInit } from '@angular/core';
import configuration from '../../config.json';

@Injectable()
export class ConfigService implements OnInit {

    ngOnInit(): void {
        this._config = configuration;
    }

    
    private _config : Configuration;
    public get config() : Configuration {
        return this._config;
    }
    

}