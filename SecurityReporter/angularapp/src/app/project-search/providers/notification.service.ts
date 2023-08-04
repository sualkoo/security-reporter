import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CustomSnackbarComponent } from '../components/custom-snackbar/custom-snackbar.component';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private snackBar: MatSnackBar) {}

  displayMessage(message: string, messageType: "info"|"success"|"warning"|"error"): void {
    this.snackBar.openFromComponent(
      CustomSnackbarComponent,
      {
        duration: 5000,
        data: {
          message: message,
          messageType: messageType,
        },
        horizontalPosition: 'center',
        verticalPosition: 'bottom',
        panelClass: 'custom-snackbar',
      }
    );
  }
}
