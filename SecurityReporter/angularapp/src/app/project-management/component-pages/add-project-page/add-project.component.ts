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
import { MatDialog } from '@angular/material/dialog';
import { AlertService } from '../../services/alert.service';
import { DatePipe } from '@angular/common';
import { max } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { AutoLogoutService } from '../../../services/auto-logout.service';

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
  isProjectNameInvalidLength: boolean = false;
  isProjectNameWhitespace: boolean = false;
  isProjectNameEmpty: boolean = true;
  isPentestValueInvalid: boolean = false;
  editedStartDate: Date;
  editedEndDate: Date;
  editedReportDueDate: Date;
  editedIKO: Date;
  editedTKO: Date;

    constructor(private addProjectService: AddProjectService, private router: Router, public alertService: AlertService, public autoLogoutService: AutoLogoutService) {
    this.editedStartDate = this.projectClass.StartDate;
    this.editedEndDate = this.projectClass.EndDate;
    this.editedReportDueDate = this.projectClass.ReportDueDate;
    this.editedIKO = this.projectClass.IKO;
    this.editedTKO = this.projectClass.TKO;
}
  @ViewChild('commentInput') commentInput?: ElementRef;

  ngOnInit() {
    this.autoLogoutService.startTimer();
  }

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
  comField = '';
  errorValue = false;

  onChildRadioValueChanged(value: number) {
    this.projectClass.PentestDuration = value;
    this.isPentestValueInvalid = value < 2 || value > 10;
  }

  onChildInputValueChanged(value: string, id: string) {

    const trimmedValue = value.trim();
    const isWhitespace = trimmedValue === '';

    this.value = value;
    switch (id) {
      case 'PN':
        this.projectClass.ProjectName = value;
        this.isProjectNameInvalidLength = value.length < 3 || value.length > 50;
        this.isProjectNameWhitespace = isWhitespace;
        this.isProjectNameEmpty = !(value.length > 0);
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
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        this.isCFCInvalidEmail = !emailRegex.test(value) && value.length > 0; 
        break;
      case 'COM':
        this.comField = value;
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
        console.log(this.projectClass.ProjectOfferStatus);
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
      const date = new Date(value);
      date.setDate(date.getDate() + 1);
      this.projectClass.StartDate = date;
      this.editedStartDate = this.correctDate(this.projectClass.StartDate);

      if (this.isEndDateSet() && this.projectClass.StartDate > this.projectClass.EndDate) {
        this.isInvalidStartDate = true;
      } else {
        this.isInvalidStartDate = false;
      }
    } else if (id === 'END') {
      const date = new Date(value);
      date.setDate(date.getDate() + 1);
      this.projectClass.EndDate = date;
      this.editedEndDate = this.correctDate(this.projectClass.EndDate);

      const maxDate = this.isRepDateSet() ? this.projectClass.ReportDueDate : this.defaultMaxDate;
      const IKOmaxDate = this.isIKOSet() ? this.projectClass.IKO : this.defaultMaxDate;
      const TKOmaxDate = this.isTKOSet() ? this.projectClass.TKO : this.defaultMaxDate;
      if (this.projectClass.EndDate < this.projectClass.StartDate || this.projectClass.EndDate > maxDate || this.projectClass.EndDate > IKOmaxDate! || this.projectClass.EndDate > TKOmaxDate!) {
        this.isInvalidEndDate = true;
      } else {
        this.isInvalidEndDate = false;
      }
    } else if (id === 'REP') {
      const date = new Date(value);
      date.setDate(date.getDate() + 1);
      this.projectClass.ReportDueDate = date;
      this.editedReportDueDate = this.correctDate(this.projectClass.ReportDueDate);

      const minDate = this.isEndDateSet() ? this.projectClass.EndDate : this.projectClass.StartDate;
      if (this.projectClass.ReportDueDate < minDate) {
        this.isInvalidReportDueDate = true;
      } else {
        this.isInvalidReportDueDate = false;
      }
    } else if (id === 'IKO') {
      const date = new Date(value);
      date.setDate(date.getDate() + 1);
      this.projectClass.IKO = date;
      this.editedIKO = this.correctDate(this.projectClass.IKO);


      const endMaxDate = this.isEndDateSet() ? this.projectClass.EndDate : this.defaultMaxDate;
      const repMaxDate = this.isRepDateSet() ? this.projectClass.ReportDueDate : this.defaultMaxDate;
      if (this.projectClass.IKO < this.projectClass.StartDate || this.projectClass.IKO > endMaxDate || this.projectClass.IKO > repMaxDate) {
        this.isInvalidIKO = true;
      } else {
        this.isInvalidIKO = false;
      }
    } else {
      const date = new Date(value);
      date.setDate(date.getDate() + 1);
      this.projectClass.TKO = date;
      this.editedTKO = this.correctDate(this.projectClass.TKO);

      const endMaxDate = this.isEndDateSet() ? this.projectClass.EndDate : this.defaultMaxDate;
      const repMaxDate = this.isRepDateSet() ? this.projectClass.ReportDueDate : this.defaultMaxDate;

      if (this.projectClass.TKO < this.projectClass.StartDate || this.projectClass.TKO > endMaxDate || this.projectClass.TKO > repMaxDate) {
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
    } else if (id == 'COM') {
      if (this.comField != '') {
        const newComment: CommentInterface = {
          text: this.comField,
        };

        this.projectClass.Comments.push(newComment);
      }
      this.comField = '';
    } else {
      if (this.cfcField != '') {
        this.projectClass.ContactForClients.push(this.cfcField);
      }
      this.cfcField = '';
    }
  }

  submit() {
    this.addProjectService.submitPMProject(this.projectClass).subscribe(
      (response) => {
        console.log('Success:', response);
        this.alertService.showSnackbar('Item added successfully.', 'Close', 'green-alert');
      },
      (error) => {
        console.log('Error:', error);

        const { title, status, errors } = error;

        this.alertService.showSnackbar('Error occured during adding an item.', 'Close', 'red-alert');

        console.log('Title:', title);
        console.log('Status Code:', status);
        console.log('Errors:', errors);
      }
    );
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

    this.submit();

  }

  getValueFromTextarea() {
    if (this.commentInput) {
      const commentText = this.commentInput.nativeElement.value;
      const trimmedComment = commentText.trim();
      const isWhitespace = trimmedComment === '';

      if (!isWhitespace) {
        this.projectClass.Comments = [{
          text: commentText
        }];
      } else {
        this.isCommentWhitespace = isWhitespace && commentText.length > 0; 
      }
    }
  }

  validationFunction() {
    if (   
      // @ts-ignore
      this.projectClass.ProjectName?.length > 2 &&
      // @ts-ignore
      this.projectClass.ProjectName?.length < 50
    ) {
       {
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
    } else if (id == 'COM') {
      const index = this.projectClass.Comments.findIndex(
        (comment: CommentInterface) => comment.text === item
      );

      if (index !== -1) {
        this.projectClass.Comments.splice(index, 1);
      }
    } else {
      this.projectClass.ContactForClients.splice(
        this.projectClass.ContactForClients.indexOf(item),
        1
      );
    }
  }

  isEndDateSet(): boolean {
    return this.projectClass.EndDate instanceof Date && this.projectClass.EndDate.getTime() !== new Date('0001-01-01').getTime();
  }

  isRepDateSet(): boolean {
    return this.projectClass.ReportDueDate instanceof Date && this.projectClass.ReportDueDate.getTime() !== new Date('0001-01-01').getTime();

  }

  isIKOSet(): boolean {
    return this.projectClass.IKO! instanceof Date && this.projectClass.IKO!.getTime() !== new Date('0001-01-01').getTime();

  }

  isTKOSet(): boolean {
    return this.projectClass.TKO! instanceof Date && this.projectClass.TKO!.getTime() !== new Date('0001-01-01').getTime();
  }

  correctDate(date: Date): Date {
    const newDate = new Date(date);
    newDate.setDate(newDate.getDate() - 1);
    return newDate;
  }
}
