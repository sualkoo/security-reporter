import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-radio-button-component',
  templateUrl: './edit-radio-button-component.component.html',
  styleUrls: ['./edit-radio-button-component.component.css'],
  standalone: true,
  imports: [MatRadioModule, FormsModule],
})
export class EditRadioButtonComponentComponent {
  @Input() title: string = '';
  radioValue: number = 0;
  inputValue: number = 5;

  @Output() valueChanged = new EventEmitter<number>();

  onRadioChange() {
    if (this.radioValue == 3) {
      this.valueChanged.emit(this.inputValue);
    } else {
      this.valueChanged.emit(this.radioValue);
    }
  }
}
