import { Component, NgModule } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { DatepickerComponent } from '../../components/datepicker-component/datepicker-component.component';
import { SelectInterface } from '../../interfaces/select-interface';
import { SelectComponentComponent } from '../../components/select-component/select-component.component';
import { InputComponentComponent } from '../../components/input-component/input-component.component';
import { RadioButtonComponentComponent } from '../../components/radio-button-component/radio-button-component.component';
import { MatButtonModule } from '@angular/material/button';
import { ProjectInterface } from '../../interfaces/project-interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-project-management',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    DatepickerComponent,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatRadioModule,
    SelectComponentComponent,
    InputComponentComponent,
    RadioButtonComponentComponent,
    MatButtonModule,
  ],
})
export class AddProjectComponent {
  ProjectStatus: SelectInterface[] = [
    { value: 'requested-0', viewValue: 'Requested' },
    { value: 'planned-1', viewValue: 'Planned' },
    { value: 'inProgress-2', viewValue: 'In progress' },
    { value: 'finished-3', viewValue: 'Finished' },
    { value: 'cancelled-4', viewValue: 'Cancelled' },
    { value: 'onHold-5', viewValue: 'On hold' },
  ];

  ProjectScope: SelectInterface[] = [
    { value: 'tbs-0', viewValue: 'TBS' },
    { value: 'sent-1', viewValue: 'Sent' },
    { value: 'confirmed-2', viewValue: 'Confirmed' },
    { value: 'signed-3', viewValue: 'Signed' },
  ];

  Questionare: SelectInterface[] = [
    { value: 'tbs-0', viewValue: 'TBS' },
    { value: 'sent-1', viewValue: 'Sent' },
    { value: 'received-2', viewValue: 'Received' },
  ];

  OfferStatus: SelectInterface[] = [
    { value: '0', viewValue: 'Waiting for Offer creation' },
    { value: '1', viewValue: 'Offer Draft sent for Review' },
    { value: '2', viewValue: 'Offer sent for signatue' },
    { value: '3', viewValue: 'Offer signed - Ready For Invoicing' },
    { value: '4', viewValue: 'Invoiced' },
    { value: '5', viewValue: 'Individual Agreement' },
    { value: '6', viewValue: 'Retest - free of charge' },
    { value: '7', viewValue: 'Other' },
    { value: '8', viewValue: 'Cancelled' },
    { value: '9', viewValue: 'Prepared' },
  ];

  value = '';

  projectClass: ProjectInterface = {
    ProjectName: '',
    StartDate: new Date('0000-01-01'),
    EndDate: new Date('0000-01-01'),
    ProjectStatus: 'TBS',
    ProjectScope: 'TBS',
    ProjectQuestionare: 'TBS',
    PentestAspects: '',
    PentestDuration: '',
    ReportDueDate: new Date('0000-01-01'),
    IKO: new Date('0000-01-01'),
    TKO: new Date('0000-01-01'),
    RequestCreated: '',
    Commments: '',
    CatsNumber: '',
    ProjectOfferStatus: 'TBS',
    WorkingTeam: [],
    ProjectLead: '',
    ReportStatus: '',
    ContactForClients: [],
  };

  wtField = '';
  cfcField = '';

  onChildInputValueChanged(value: string, id: string) {
    this.value = value;
    switch (id) {
      case 'PN':
        this.projectClass.ProjectName = value;
        break;
      case 'PA':
        this.projectClass.PentestAspects = value;
        break;
      case 'PL':
        this.projectClass.ProjectLead = value;
        break;
      case 'WT':
        this.wtField = value;
        break;
      case 'RS':
        this.projectClass.ReportStatus = value;
        break;
      case 'CFC':
        this.cfcField = value;
        break;
    }
  }

  onChildButtonValueChanged(id: string) {
    if (id == 'WT') {
      this.projectClass.WorkingTeam.push(this.wtField);
      this.wtField = '';
    } else {
      this.projectClass.ContactForClients.push(this.cfcField);
      this.cfcField = '';
    }
  }

  sendRequest() {
    for (const [key, value] of Object.entries(this.projectClass)) {
      if (this.projectClass.hasOwnProperty(key)) {
        if (
          // @ts-ignore
          this.projectClass[key] === '' ||
          // @ts-ignore
          this.projectClass[key] === 'TBS'
        ) {
          // @ts-ignore
          this.projectClass[key] = null;
        }
      }
      // @ts-ignore
      if (Array.isArray(this.projectClass[key])) {
        // @ts-ignore

        if (this.projectClass[key].length == 0) {
          // @ts-ignore
          this.projectClass[key] = null;
        }
      }
      // @ts-ignore
      if (this.projectClass[key] instanceof Date) {
        // @ts-ignore
        this.projectClass[key] = `${this.projectClass[key]
          .getUTCFullYear()
          .toString()
          // @ts-ignore
          .padStart(4, '0')}-${(this.projectClass[key].getUTCMonth() + 1)
          .toString()
          // @ts-ignore
          .padStart(2, '0')}-${this.projectClass[key]
          .getUTCDate()
          .toString()
          .padStart(2, '0')}`;
      }
    }

    console.log(Object.keys(this.projectClass).length);

    if (this.projectClass.StartDate && this.projectClass.EndDate) {
      if (this.projectClass.StartDate <= this.projectClass.EndDate) {
        console.log(this.projectClass);
      } else {
        console.log('Bad date');
      }
    }
  }
}
