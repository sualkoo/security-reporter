import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { BacklogComponentComponent } from '../../components/backlog-component/backlog-component.component';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AlertService } from '../../../project-management/services/alert.service';
import { GetEmailService } from '../../../project-editing/services/get-email.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css'],
  standalone: true,
  imports: [MatButtonModule, BacklogComponentComponent, MatTabsModule],
})
export class MyProfileComponent {
  role = '';
  email = '';

  constructor(
    private getEmailService: GetEmailService,
    private router: Router,
    private snackBar: MatSnackBar,
    private alertService: AlertService,
    private authService: AuthService
  ) {
    this.getRole();
    this.getEmail();
  }

  async getRole() {
    this.role = await this.authService.getRole();
  }

  async getEmail() {
    this.email = await this.getEmailService.getEmail();
  }

  logoutPage() {
    if (this.authService.logout()) {
      this.alertService.showSnackbar(
        'You are being logged out',
        'Close',
        'red-alert',
        5000
      );
    } else {
      this.alertService.showSnackbar(
        'Error happened during logout',
        'Close',
        'red-alert',
        5000
      );
    }

    this.snackBar.open('You are being logged out.', 'Close', {
      duration: 5000,
      panelClass: 'red-alert',
    });
    setTimeout(() => {
      window.location.href = '/welcome';
    }, 1000);
  }
}
