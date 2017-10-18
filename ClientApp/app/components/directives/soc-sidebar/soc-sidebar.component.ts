import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'soc-sidebar',
    templateUrl: 'soc-sidebar.component.html'
})

export class SocSidebarComponent implements OnInit {
    private formVisible = true;

    constructor() { }

    ngOnInit() { 

    }

    public collapseForm = () => {
        this.formVisible = this.formVisible;
    }
}