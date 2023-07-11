import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatNativeDateModule} from '@angular/material/core';
import {NgFor} from '@angular/common';
import {MatSelectModule} from '@angular/material/select';
import {MatRadioModule} from '@angular/material/radio';

interface Food {
  value: string;
  viewValue: string;
}

interface ProjectStatus {
  value: string;
  viewValue: string;
}


@Component({
  selector: 'app-project-management',
  templateUrl: './project-management.component.html',
  styleUrls: ['./project-management.component.css'], 
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, 
    MatDatepickerModule, MatNativeDateModule, NgFor, MatSelectModule, MatRadioModule]
})
export class ProjectManagementComponent {
  ProjectStatus: ProjectStatus[] = [
    {value: 'requested-0', viewValue: 'Requested'},
    {value: 'planned-1', viewValue: 'Planned'},
    {value: 'inProgress-2', viewValue: 'In progress'},
    {value: 'finished-2', viewValue: 'Finished'},
    {value: 'cancelled-2', viewValue: 'Cancelled'},
    {value: 'onHold-2', viewValue: 'On hold'},
  ];

  ProjectScope: ProjectStatus[] = [
    {value: 'tbs-0', viewValue: 'TBS'},
    {value: 'sent-1', viewValue: 'Sent'},
    {value: 'confirmed-2', viewValue: 'Confirmed'},
    {value: 'signed-2', viewValue: 'Signed'},
  ];

  Questionare: ProjectStatus[] = [
    {value: 'tbs-0', viewValue: 'TBS'},
    {value: 'sent-1', viewValue: 'Sent'},
    {value: 'received-2', viewValue: 'Received'},
  ];

}