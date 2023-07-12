import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { NgFor } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { DatepickerComponent } from '../../components/datepicker-component/datepicker-component.component';
import { SelectInterface } from '../../interfaces/select-interface';
import { SelectComponentComponent } from '../../components/select-component/select-component.component';
import { InputComponentComponent } from '../../components/input-component/input-component.component';
import { RadioButtonComponentComponent } from '../../components/radio-button-component/radio-button-component.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-project-management',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    DatepickerComponent,
    MatDatepickerModule,
    MatNativeDateModule,
    NgFor,
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
}
