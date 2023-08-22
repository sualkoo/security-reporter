import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AutoLogoutService } from '../services/auto-logout.service';
import { AlertService } from '../project-management/services/alert.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css'],
})
export class LandingPageComponent {
  constructor(private router: Router, private alertService: AlertService, private logOut: AuthService) { }


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
  }
}
