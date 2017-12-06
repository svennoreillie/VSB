import { Input } from "@angular/core";
import { Component } from "@angular/core";

@Component({
    selector: "soc-panel",
    templateUrl: "soc-panel.component.html"
})

export class SocPanelComponent {

    private visible: boolean = true;
    @Input() public loading: boolean = false;
    @Input() public collapsable: boolean = true;
    @Input() public paneltitle: string;

    public toggle() {
        this.visible = !this.visible;
    }
}

export enum SocPanelLoadingType {
    bar,
    spinner
}