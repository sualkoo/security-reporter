import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.css'],
  standalone: true,
  imports: [MatInputModule, MatFormFieldModule, FormsModule]
})
export class InputComponent {

  @Input() title: string = '';
  @Input() inputValue: string = '';
  @Output() valueChanged = new EventEmitter<string>();

  onInputChange() {
    this.valueChanged.emit(this.inputValue);
  }
}
