import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-input-component',
  templateUrl: './input-component.component.html',
  standalone: true,
  styleUrls: ['./input-component.component.css'],
  imports: [MatFormFieldModule, MatInputModule, NgIf, FormsModule],
})
export class InputComponentComponent {
  @Input() title: string = '';
  @Input() inputValue: string = '';
  @Output() valueChanged = new EventEmitter<string>();

  onInputChange() {
    this.valueChanged.emit(this.inputValue);
  }
}
