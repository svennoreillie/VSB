import { Input } from "@angular/core";
import { Component } from "@angular/core";

@Component({
    selector: "soc-panel",
    templateUrl: "soc-panel.component.html"
})

export class SocPanelComponent {

    @Input() public loading: boolean = false;
    @Input() public collapsable: boolean = true;
    @Input() public collapsed: boolean = false;
    @Input() public paneltitle: string;

    public toggle() {
        this.collapsed = !this.collapsed;
    }
}
