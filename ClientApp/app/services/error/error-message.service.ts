import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class ErrorMessageService {

    constructor(private snackBar: MatSnackBar) { }

    public showError(message: string): void {
        this.snackBar.open(message, undefined, {
            duration: 2500,
            extraClasses: ['soc-error']
          });
    }
} 