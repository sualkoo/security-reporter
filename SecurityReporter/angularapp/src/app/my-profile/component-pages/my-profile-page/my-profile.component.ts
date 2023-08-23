import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { BacklogComponentComponent } from '../../components/backlog-component/backlog-component.component';
import { GetRoleService } from 'src/app/shared/services/get-role.service';
import { GetEmailService } from 'src/app/shared/services/get-email.service';

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
    private getEmailService: GetEmailService
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
}
