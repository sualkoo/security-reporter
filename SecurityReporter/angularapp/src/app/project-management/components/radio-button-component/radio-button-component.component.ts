import { Component, Input, Output, EventEmitter, NgModule } from '@angular/core';
import { MatRadioModule } from '@angular/material/radio';
import { CommonModule, NgIf } from '@angular/common'
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-radio-button-component',
  templateUrl: './radio-button-component.component.html',
  styleUrls: ['./radio-button-component.component.css'],
  standalone: true,
  imports: [MatRadioModule, FormsModule, CommonModule, MatInputModule],
})
export class RadioButtonComponentComponent {
  @Input() title: string = '';
  radioValue: number = 0;
  inputValue: number = 5;
  selectedOption: number = 0;

  @Output() valueChanged = new EventEmitter<number>();

  onRadioChange() {
    if (this.radioValue == 3) {
      this.valueChanged.emit(this.inputValue);
    } else {
      this.valueChanged.emit(this.radioValue);
    }
  }
}
