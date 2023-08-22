import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AlertService } from '../project-management/services/alert.service';

@Component({
  selector: 'app-after-login-page',
  templateUrl: './after-login-page.component.html',
  styleUrls: ['./after-login-page.component.css']
})
export class AfterLoginPageComponent {
  constructor(private router: Router, private snackBar: MatSnackBar, private logOut: AuthService, private alertService: AlertService) { }


  navigateToPage(page: string): void {
    this.router.navigate([page]);
  }

  logoutPage() {
    if (this.logOut.logout()) {
      this.alertService.showSnackbar('You are being logged out', 'Close', 'red-alert', 5000);
    }
    else {
      this.alertService.showSnackbar('Error happened during logout', 'Close', 'red-alert', 5000);
    }

    this.snackBar.open('You are being logged out.', 'Close', {
      duration: 5000,
      panelClass: 'red-alert'
    });
    setTimeout(() => {
      window.location.href = '/welcome';
    }, 1000);
  }

}
