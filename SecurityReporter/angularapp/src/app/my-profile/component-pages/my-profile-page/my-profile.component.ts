import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { BacklogComponentComponent } from '../../components/backlog-component/backlog-component.component';
import { GetRoleService } from 'src/app/shared/services/get-role.service';
import { GetEmailService } from 'src/app/shared/services/get-email.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LogoutService } from '../../../services/logout.service';
import { AlertService } from '../../../project-management/services/alert.service';

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
    private getRoleService: GetRoleService,
    private getEmailService: GetEmailService,
    private router: Router,
    private snackBar: MatSnackBar,
    private logOut: LogoutService,
    private alertService: AlertService
  ) {
    this.getRole();
    this.getEmail();
  }

  async getRole() {
    this.role = await this.getRoleService.getRole();
  }

  async getEmail() {
    this.email = await this.getEmailService.getEmail();
  }

  logoutPage() {
    if (this.logOut.logout()) {
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
