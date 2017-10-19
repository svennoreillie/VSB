import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'soc-sidebar',
    templateUrl: 'soc-sidebar.component.html'
})

export class SocSidebarComponent implements OnInit {
    private formVisible: boolean = true;

    constructor() { }

    ngOnInit() { 

    }

    public collapseForm = () => {
        this.formVisible = this.formVisible;
    }
}