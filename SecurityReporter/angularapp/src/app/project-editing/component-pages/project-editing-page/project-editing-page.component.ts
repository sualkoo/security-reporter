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
  project!: ProjectInterface;
  projectForm!: FormGroup;

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

  getProjectDetails(projectId: string) {
    this.getProjectService.getProjectById(projectId).subscribe(
      (projectData: ProjectInterface) => {
        this.project = projectData;
        console.log(projectData);
        // Initialize the projectForm FormGroup here after getting the project data
        this.projectForm = this.formBuilder.group({
          projectName: [this.project?.ProjectName || 'SomKar', Validators.required],
          // Add other form controls for other properties of ProjectInterface if needed
        });
      },
      (error) => {
        console.error('Error fetching project data:', error);
      }
    );
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
