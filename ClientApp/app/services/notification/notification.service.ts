import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';    


@Injectable()
export class NotificationService {

    constructor(private snackBar: MatSnackBar) { }

    public showError(message: string): void {
        this.snackBar.open(message, undefined, {
            duration: 2000,
            extraClasses: ['soc-error']
          });
    }
    public showInfo(message: string): any {
        this.snackBar.open(message, undefined, {
            duration: 2000
          });
    }
} 