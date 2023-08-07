import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css'],
})
export class LandingPageComponent {
  constructor(private router: Router, private snackBar: MatSnackBar) { }

  navigateToPage(page: string): void {
    this.router.navigate([page]);
  }

  logoutPage() {
    // Show snackbar message
    this.snackBar.open('You are being logged out.', 'Close', {
      duration: 5000, 
      panelClass: 'red-alert'
    });
    setTimeout(() => {
      window.location.href = 'https://localhost:4200';
    }, 1000);
  }
}
