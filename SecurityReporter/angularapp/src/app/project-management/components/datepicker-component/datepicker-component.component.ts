import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewChild,
} from '@angular/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule, FormControl, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';

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
    FormsModule,
    NgIf,
  ],
})
export class DatepickerComponent {
  @ViewChild('picker') picker: any;
  @Input() title: string = '';
  @Input() minDate: Date;
  @Input() maxDate: Date;

  inputValue: Date = new Date('0001-01-01');


  constructor() {
    // Set the minimum to January 1st 20 years in the past and December 31st a year in the future.
    const currentYear = new Date().getFullYear();
    this.minDate = new Date(new Date().setDate(new Date().getDate() - 1));
    this.maxDate = new Date(currentYear + 1, 11, 31);
    this.compareDates(this.minDate, this.maxDate);
  }

  ngOnInit() {
    if (this.minDate < new Date(new Date().setDate(new Date().getDate() - 1))) {
      this.minDate = new Date(new Date().setDate(new Date().getDate() - 1));
    }
  }

  compareDates(start: Date, end: Date) {
    if (start > end) {
    }
  }

  @Output() valueChanged = new EventEmitter<Date>();

  onDateChange() {
    this.valueChanged.emit(this.inputValue);
  }
}
