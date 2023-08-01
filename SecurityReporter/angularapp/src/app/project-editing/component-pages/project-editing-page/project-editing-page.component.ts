import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule, NgIf } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { InputComponentComponent } from '../../../project-management/components/input-component/input-component.component';
import { RadioButtonComponentComponent } from '../../../project-management/components/radio-button-component/radio-button-component.component';
import { DatepickerComponent } from '../../../project-management/components/datepicker-component/datepicker-component.component';
import { AddProjectComponent } from '../../../project-management/component-pages/add-project-page/add-project.component';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';
import { UpdateProjectService } from '../../services/update-project.service';
import { AddProjectService } from '../../../project-management/services/add-project.service';
import { Router } from '@angular/router';
import { AlertService } from '../../../project-management/services/alert.service';
import { MatTooltipModule } from '@angular/material/tooltip';
import { GetProjectService } from '../../services/get-project.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-editing-page',
  templateUrl: './project-editing-page.component.html',
  styleUrls: ['../../../project-management/component-pages/add-project-page/add-project.component.css', './project-editing-page.component.css'],
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
    MatTooltipModule
  ],
})
export class ProjectEditingPageComponent extends AddProjectComponent {
  projectId!: string;
  projectForm!: FormGroup;

  loadedProject: any;

  constructor(private route: ActivatedRoute, addProjectService: AddProjectService,
    router: Router, alertService: AlertService,
    private updateProjectService: UpdateProjectService,
    private getProjectService: GetProjectService,
    private formBuilder: FormBuilder) {
    super(addProjectService, router, alertService);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.projectId = params['id'];
      this.getProjectDetails(this.projectId);
    });

    this.projectForm = this.formBuilder.group({
      projectName: [this.project?.ProjectName || 'SomKar', Validators.required],
      // Add other form controls for other properties of ProjectInterface if needed
    });

  }

  mapJsonToProjectInterface(jsonData: any): ProjectInterface {
  return {
    id: jsonData.id,
    ProjectName: jsonData.projectName,
    StartDate: jsonData.startDate ? jsonData.startDate : new Date('0001-01-01'),
    EndDate: jsonData.endDate ? jsonData.endDate : new Date('0001-01-01'),
    ProjectStatus: jsonData.projectSatus,
    ProjectScope: jsonData.projectScope,
    ProjectQuestionare: jsonData.projectQuestionare,
    PentestAspects: jsonData.pentestAspects,
    PentestDuration: jsonData.pentestDuration,
    ReportDueDate: jsonData.reportDueDate ? jsonData.reportDueDate : new Date('0001-01-01'),
    IKO: jsonData.iko ? jsonData.iko : new Date('0001-01-01'),
    TKO: jsonData.tko ? jsonData.tko : new Date('0001-01-01'),
    RequestCreated: jsonData.requestCreated,
    Comments: jsonData.comments,
    CatsNumber: jsonData.castNumber,
    ProjectOfferStatus: jsonData.projectOfferStatus,
    WorkingTeam: jsonData.workingTeam,
    ProjectLead: jsonData.projectLead,
    ReportStatus: jsonData.reportStatus,
    ContactForClients: jsonData.contactForClients
  };
}

  async getProjectDetails(projectId: string) {
    var projectData = await this.getProjectService.getProjectById(projectId);
    this.projectClass = this.mapJsonToProjectInterface(projectData);
    console.log(this.projectClass);
  }

  getStatusString(status: number): string {
    switch (status) {
      case 1:
        return 'Requested';
      case 2:
        return 'Planned';
      case 3:
        return 'In progress';
      case 4:
        return 'Finished';
      case 5:
        return 'Cancelled';
      case 6:
        return 'On hold';
      default:
        return 'None';
    }
  }

  getQuestionareString(questionare: number): string {
    switch (questionare) {
      case 1:
        return 'TBS';
      case 2:
        return 'Sent';
      case 3:
        return 'Received';
      default:
        return 'None';
    }
  }

  getScopeString(scope: number): string {
    switch (scope) {
      case 1:
        return 'TBS';
      case 2:
        return 'Sent';
      case 3:
        return 'Confirmed';
      case 4:
        return 'Signed';
      default:
        return 'None';
    }
  }

  getOfferStatusString(status: number): string {
    switch (status) {
      case 1:
        return 'Waiting for Offer creation';
      case 2:
        return 'Offer Draft sent for Review';
      case 3:
        return 'Offer sent for signatue';
      case 4:
        return 'Offer signed - Ready For Invoicing';
      case 5:
        return 'Invoiced';
      case 6:
        return 'Individual Agreement';
      case 7:
        return 'Retest - free of charge';
      case 8:
        return 'Other';
      case 9:
        return 'Cancelled';
      case 10:
        return 'Prepared';
      default:
        return 'None';
    }
  }

  submit() {
    this.updateProjectService.updateProject(this.projectClass).subscribe(
      (response) => {
        console.log('Success:', response);
        this.alertService.showSnackbar('Item saved successfully.', 'Close', 'green-alert');
      },
      (error) => {
        console.log('Error:', error);

        const { title, status, errors } = error;

        this.alertService.showSnackbar('Error occured during saving an item.', 'Close', 'red-alert');

        console.log('Title:', title);
        console.log('Status Code:', status);
        console.log('Errors:', errors);
      }
    );
  }
}
