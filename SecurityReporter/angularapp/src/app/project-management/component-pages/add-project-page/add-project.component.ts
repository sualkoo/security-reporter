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
import { CommentInterface } from '../../interfaces/comment-interface';
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
import { Router } from '@angular/router';

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
  defaultMaxDate: Date = new Date('3000-12-31');
  isInvalidStartDate = false;
  isInvalidEndDate = false;
  isInvalidReportDueDate = false;
  isInvalidIKO = false;
  isInvalidTKO = false;
  isProjectLeadWhitespace: boolean = false;
  isPentestAspectWhitespace: boolean = false;
  isWorkingTeamWhitespace: boolean = false;
  isReportStatusWhitespace: boolean = false;
  isCFCWhitespace: boolean = false;
  isCatsNumberWhitespace: boolean = false;
  isCommentWhitespace: boolean = false;
  isCFCInvalidEmail: boolean = false;


  constructor(private addProjectService: AddProjectService, private router: Router) {}
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
    ProjectStatus: projectStatusIndex['Requested'],
    ProjectScope: projectScopeIndex['TBS'],
    ProjectQuestionare: projectQuestionareIndex['TBS'],
    PentestAspects: '',
    PentestDuration: -1,
    ReportDueDate: new Date('0001-01-01'),
    IKO: new Date('0001-01-01'),
    TKO: new Date('0001-01-01'),
    RequestCreated: '',
    Comments: [],
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

    const trimmedValue = value.trim();
    const isWhitespace = trimmedValue === '';

    this.value = value;
    switch (id) {
      case 'PN':
        this.projectClass.ProjectName = value;
        break;
      case 'PA':
        this.projectClass.PentestAspects = value;
        this.isPentestAspectWhitespace = isWhitespace && value.length > 0; 
        break;
      case 'PL':
        this.projectClass.ProjectLead = value;
        this.isProjectLeadWhitespace = isWhitespace && value.length > 0;
        break;
      case 'WT':
        this.wtField = value;
        this.isWorkingTeamWhitespace = isWhitespace && value.length > 0; 
        break;
      case 'CFC':
        this.cfcField = value;
        this.isCFCWhitespace = isWhitespace && value.length > 0;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Email regex pattern
        this.isCFCInvalidEmail = !emailRegex.test(value) && value.length > 0; // Check if the email is invalid
        break;
      case 'RS':
        this.projectClass.ReportStatus = value;
        this.isReportStatusWhitespace = isWhitespace && value.length > 0; 
        break;
      case 'CN':
        this.projectClass.CatsNumber = value;
        this.isCatsNumberWhitespace = isWhitespace && value.length > 0; 
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
    if (id === 'STR') {
      this.projectClass.StartDate = value;
      // Check if the start date is after the end date
      if (this.isEndDateSet() && this.projectClass.StartDate > this.projectClass.EndDate) {
        this.isInvalidStartDate = true;
      } else {
        this.isInvalidStartDate = false;
      }
    } else if (id === 'END') {
      this.projectClass.EndDate = value;
      // Check if the end date is before the start date
      if (this.projectClass.EndDate < this.projectClass.StartDate) {
        this.isInvalidEndDate = true;
      } else {
        this.isInvalidEndDate = false;
      }
    } else if (id === 'REP') {
      this.projectClass.ReportDueDate = value;
      // Check if Report Due Date is before the start date or end date
      const minDate = this.isEndDateSet() ? this.projectClass.EndDate : this.projectClass.StartDate;
      if (this.projectClass.ReportDueDate < minDate) {
        this.isInvalidReportDueDate = true;
      } else {
        this.isInvalidReportDueDate = false;
      }
    } else if (id === 'IKO') {
      this.projectClass.IKO = value;
      // Check if IKO is outside the range of Start Date and End Date
      if (this.projectClass.IKO < this.projectClass.StartDate || this.projectClass.IKO > this.projectClass.EndDate) {
        this.isInvalidIKO = true;
      } else {
        this.isInvalidIKO = false;
      }
    } else {
      this.projectClass.TKO = value;
      // Check if TKO is outside the range of Start Date and End Date
      if (this.projectClass.TKO < this.projectClass.StartDate || this.projectClass.TKO > this.projectClass.EndDate) {
        this.isInvalidTKO = true;
      } else {
        this.isInvalidTKO = false;
      }
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
          this.projectClass[key] === -1 ||
          // @ts-ignore
          this.projectClass[key] === []
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

    this.addProjectService.submitPMProject(this.projectClass).subscribe(
      (response) => {
        console.log('Success:', response);
      },
      (error) => {
        console.log('Error:', error);

        const { title, status, errors } = error;


        console.log('Title:', title);
        console.log('Status Code:', status);
        console.log('Errors:', errors);
      }
    );

  }

  getValueFromTextarea() {
    if (this.commentInput) {
      const commentText = this.commentInput.nativeElement.value;
      const trimmedComment = commentText.trim();
      const isWhitespace = trimmedComment === '';

      if (!isWhitespace) {
        // Set the comment if it's not just whitespace
        this.projectClass.Comments = [{
          text: commentText
        }];
      } else {
        // If comment is empty or contains only whitespace, remove the comment
        this.isCommentWhitespace = isWhitespace && commentText.length > 0; 
      }
    }
  }

  validationFunction() {
    // @ts-ignore
    
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
        this.router.navigate(['/list-projects']);
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

  isEndDateSet(): boolean {
    return this.projectClass.EndDate.getTime() !== new Date('0001-01-01').getTime();
  }

  
}
