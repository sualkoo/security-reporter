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
import { UpdateProjectService } from '../../services/update-project.service';
import { AddProjectService } from '../../../project-management/services/add-project.service';
import { Router } from '@angular/router';
import { AlertService } from '../../../project-management/services/alert.service';

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
  constructor(addProjectService: AddProjectService, router: Router, alertService: AlertService, private updateProjectService: UpdateProjectService) {
    super(addProjectService, router, alertService);
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
