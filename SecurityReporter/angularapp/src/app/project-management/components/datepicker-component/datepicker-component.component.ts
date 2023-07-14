import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { FormsModule } from '@angular/forms';

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
  ],
})
export class DatepickerComponent {
  @Input() title: string = '';
  inputValue: Date = new Date();

  @Output() valueChanged = new EventEmitter<Date>();

  onDateChange() {
    this.valueChanged.emit(this.inputValue);
    console.log(this.inputValue);
  }
}
