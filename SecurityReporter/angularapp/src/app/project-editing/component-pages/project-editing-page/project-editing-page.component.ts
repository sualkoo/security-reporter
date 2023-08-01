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
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { InputComponentComponent } from '../../../project-management/components/input-component/input-component.component';
import { RadioButtonComponentComponent } from '../../../project-management/components/radio-button-component/radio-button-component.component';
import { DatepickerComponent } from '../../../project-management/components/datepicker-component/datepicker-component.component';
import { AddProjectComponent } from '../../../project-management/component-pages/add-project-page/add-project.component';
<<<<<<< HEAD
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';
=======
import { UpdateProjectService } from '../../services/update-project.service';
import { AddProjectService } from '../../../project-management/services/add-project.service';
import { Router } from '@angular/router';
>>>>>>> 1a6529939a9d7ba314e970eedb220701f49d7e99

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
  ],
})
export class ProjectEditingPageComponent extends AddProjectComponent {
<<<<<<< HEAD
  projectId!: string;
  project!: ProjectInterface;


=======
  constructor(addProjectService: AddProjectService, router: Router, private updateProjectService: UpdateProjectService) {
    super(addProjectService, router);
  }

  submit() {
    this.updateProjectService.updateProject(this.projectClass).subscribe(
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
>>>>>>> 1a6529939a9d7ba314e970eedb220701f49d7e99
}
