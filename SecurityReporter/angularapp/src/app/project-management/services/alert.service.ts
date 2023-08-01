import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  constructor(private snackBar: MatSnackBar) { }

  showSnackbar(message: string, action: string, cssClass: string, duration: number = 3000) {
    this.snackBar.open(message, action, {
      duration: duration,
      panelClass: ['custom-alert', cssClass], 
    });
  }
}
