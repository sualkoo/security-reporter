import { Component, ViewChild, ElementRef } from '@angular/core';
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
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import {
  ProjectInterface,
  projectOfferStatusIndex,
  projectQuestionareIndex,
  projectScopeIndex,
  projectStatusIndex,
} from '../../interfaces/project-interface';
import { CommonModule, NgIf } from '@angular/common';
import { v4 as uuidv4 } from 'uuid';
import { MatIconModule } from '@angular/material/icon';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AddProjectService } from '../../services/add-project.service';

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
    NgIf,
    MatListModule,
    MatIconModule,
  ],
})
export class AddProjectComponent {
  constructor(private addProjectService: AddProjectService) {}
  @ViewChild('commentInput') commentInput?: ElementRef;

  ProjectStatus: SelectInterface[] = [
    { value: 'Requested', viewValue: 'Requested' },
    { value: 'Planned', viewValue: 'Planned' },
    { value: 'In progress', viewValue: 'In progress' },
    { value: 'Finished', viewValue: 'Finished' },
    { value: 'Cancelled', viewValue: 'Cancelled' },
    { value: 'On hold', viewValue: 'On hold' },
  ];

  ProjectScope: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'Sent', viewValue: 'Sent' },
    { value: 'Confirmed', viewValue: 'Confirmed' },
    { value: 'Signed', viewValue: 'Signed' },
  ];

  Questionare: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'Sent', viewValue: 'Sent' },
    { value: 'Received', viewValue: 'Received' },
  ];

  OfferStatus: SelectInterface[] = [
    {
      value: 'Waiting for Offer creation',
      viewValue: 'Waiting for Offer creation',
    },
    {
      value: 'Offer Draft sent for Review',
      viewValue: 'Offer Draft sent for Review',
    },
    { value: 'Offer sent for signatue', viewValue: 'Offer sent for signatue' },
    {
      value: 'Offer signed - Ready For Invoicing',
      viewValue: 'Offer signed - Ready For Invoicing',
    },
    { value: 'Invoiced', viewValue: 'Invoiced' },
    { value: 'Individual Agreement', viewValue: 'Individual Agreement' },
    { value: 'Retest - free of charge', viewValue: 'Retest - free of charge' },
    { value: 'Other', viewValue: 'Other' },
    { value: 'Cancelled', viewValue: 'Cancelled' },
    { value: 'Prepared', viewValue: 'Prepared' },
  ];

  value = '';

  projectClass: ProjectInterface = {
    id: uuidv4(),
    ProjectName: '',
    StartDate: new Date('0001-01-01'),
    EndDate: new Date('0001-01-01'),
    ProjectStatus: projectStatusIndex['TBS'],
    ProjectScope: projectScopeIndex['TBS'],
    ProjectQuestionare: projectQuestionareIndex['TBS'],
    PentestAspects: '',
    PentestDuration: -1,
    ReportDueDate: new Date('0001-01-01'),
    IKO: new Date('0001-01-01'),
    TKO: new Date('0001-01-01'),
    Commments: '',
    CatsNumber: '',
    ProjectOfferStatus: projectOfferStatusIndex['TBS'],
    WorkingTeam: [],
    ProjectLead: '',
    ReportStatus: '',
    ContactForClients: [],
  };

  wtField = '';
  cfcField = '';
  errorValue = false;

  onChildRadioValueChanged(value: number) {
    this.projectClass.PentestDuration = value;
  }

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
      case 'CFC':
        this.cfcField = value;
        break;
      case 'RS':
        this.projectClass.ReportStatus = value;
        break;

      case 'CN':
        this.projectClass.CatsNumber = value;
        break;
      case 'OS':
        // @ts-ignore
        this.projectClass.ProjectOfferStatus = projectOfferStatusIndex[value];
        break;
      case 'PST':
        // @ts-ignore
        this.projectClass.ProjectStatus = projectStatusIndex[value];
        break;
      case 'PSC':
        // @ts-ignore
        this.projectClass.ProjectScope = projectScopeIndex[value];
        break;
      case 'QUE':
        // @ts-ignore
        this.projectClass.ProjectQuestionare = projectQuestionareIndex[value];
        break;
    }
  }

  onChildDateValueChanged(value: Date, id: string) {
    if (id == 'STR') {
      this.projectClass.StartDate = value;
    } else if (id == 'END') {
      this.projectClass.EndDate = value;
    } else if (id == 'REP') {
      this.projectClass.ReportDueDate = value;
    } else if (id == 'IKO') {
      this.projectClass.IKO = value;
    } else {
      this.projectClass.TKO = value;
    }
  }

  onChildButtonValueChanged(id: string) {
    if (id == 'WT') {
      if (this.wtField != '') {
        this.projectClass.WorkingTeam.push(this.wtField);
      }
      this.wtField = '';
    } else {
      if (this.cfcField != '') {
        this.projectClass.ContactForClients.push(this.cfcField);
      }
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
          this.projectClass[key] === 'TBS' ||
          // @ts-ignore
          this.projectClass[key] === -1
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

    console.log(this.projectClass.id);

    if (this.projectClass.StartDate && this.projectClass.EndDate) {
      if (this.projectClass.StartDate <= this.projectClass.EndDate) {
        console.log(this.projectClass);
      } else {
        console.log('Bad date');
      }
    }

    this.addProjectService.submitPMProject(this.projectClass);
  }

  getValueFromTextarea() {
    if (this.commentInput) {
      this.projectClass.Commments = this.commentInput.nativeElement.value;
    }
  }

  validationFunction() {
    // @ts-ignore
    if (this.projectClass.StartDate > this.projectClass.EndDate) {
      this.errorValue = true;
    }
    // @ts-ignore

    if (this.projectClass.EndDate > this.projectClass.ReportDueDate) {
      this.errorValue = true;
    }

    if (
      // @ts-ignore

      this.projectClass.StartDate <= this.projectClass.EndDate &&
      // @ts-ignore

      this.projectClass.EndDate <= this.projectClass.ReportDueDate &&
      // @ts-ignore

      this.projectClass.ProjectName?.length > 3 &&
      // @ts-ignore

      this.projectClass.ProjectName?.length < 50
    ) {
      if (
        // @ts-ignore

        this.projectClass.ProjectName[0].toUpperCase() ==
        // @ts-ignore

        this.projectClass.ProjectName[0]
      ) {
        this.sendRequest();
        return;
      }
    }
    this.errorValue = true;
  }

  deleteItem(item: string, id: string) {
    if (id == 'WT') {
      this.projectClass.WorkingTeam.splice(
        this.projectClass.WorkingTeam.indexOf(item),
        1
      );
    } else {
      this.projectClass.ContactForClients.splice(
        this.projectClass.ContactForClients.indexOf(item),
        1
      );
    }
  }

  checkDateValidity() {
    //here will be code to make datepicker red
  }
}
