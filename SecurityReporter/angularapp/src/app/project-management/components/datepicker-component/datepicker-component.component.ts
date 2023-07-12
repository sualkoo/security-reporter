import { Component, Input, Output } from '@angular/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';

/** @title Basic datepicker */
@Component({
  selector: 'datepicker-component',
  templateUrl: 'datepicker-component.component.html',
  styleUrls: ['./datepicker-component.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
})
export class DatepickerComponent {
  @Input() title: string = '';
}
