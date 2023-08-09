import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-after-login-page',
  templateUrl: './after-login-page.component.html',
  styleUrls: ['./after-login-page.component.css']
})
export class AfterLoginPageComponent {
  constructor(private router: Router, private snackBar: MatSnackBar) { }


  navigateToPage(page: string): void {
    this.router.navigate([page]);
  }

  logoutPage() {
    this.snackBar.open('You are being logged out.', 'Close', {
      duration: 5000,
      panelClass: 'red-alert'
    });
    setTimeout(() => {
      window.location.href = '/welcome';
    }, 1000);
  }

}
