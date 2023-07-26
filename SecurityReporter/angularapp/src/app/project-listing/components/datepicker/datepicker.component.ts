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


@Component({
  selector: 'app-datepicker',
  templateUrl: './datepicker.component.html',
  styleUrls: ['./datepicker.component.css'],
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
export class FiltersDatepickerComponent {
  @ViewChild('picker') picker: any;
  @Input() title: string = '';
  @Input() inputValue: Date = new Date(new Date().setDate(new Date().getDate()));

  @Output() valueChanged = new EventEmitter<Date>();

  onDateChange() {
    //const formattedDate = this.formatDate(this.inputValue);
    this.valueChanged.emit(this.inputValue);
  }

  //private formatDate(date: Date): Date {
  //  const year = date.getFullYear();
  //  const month = date.getMonth() + 1;
  //  const day = date.getDate();
  //  return new Date(year, month - 1, day); // Subtract 1 from month because Date object's month is zero-indexed
  //}
}
