import { Component, Inject, OnInit } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-custom-snackbar',
  templateUrl: './custom-snackbar.component.html',
  styleUrls: ['./custom-snackbar.component.css'],
  standalone: true,
  imports: [MatIconModule],
})
export class CustomSnackbarComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any, public snackBarRef: MatSnackBarRef<CustomSnackbarComponent>) {
  }

  get getIcon() {
    switch (this.data.messageType) {
      case 'info':
        return 'info';
      case 'error':
        return 'error';
      case 'warning':
        return 'warning';
      case 'success':
        return 'check'
      default:
        return 'warning';
    }
  }

  get getTitle() {
    switch (this.data.messageType) {
      case 'info':
        return 'Attention';
      case 'error':
        return 'Something went wrong';
      case 'warning':
        return 'Warning';
      case 'success':
        return 'Succesfull'
      default:
        return 'warning';
    }
  } 


}
