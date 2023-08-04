import { Component, Input, Output, EventEmitter } from '@angular/core';
import { NgFor } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { SelectInterface } from '../../interfaces/select-interface';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-select-component',
  templateUrl: './select-component.component.html',
  styleUrls: ['./select-component.component.css'],
  standalone: true,
  imports: [NgFor, MatSelectModule, FormsModule],
})
export class SelectComponentComponent {
  @Input() option: SelectInterface[] = [];
  @Input() title: string = '';
  @Input() inputValue: string = '';

  @Output() valueChanged = new EventEmitter<string>();

  onSelectChange() {
    this.valueChanged.emit(this.inputValue);
  }

  selectedValue: string | null = null;

  onValueChange(value: string) {
    this.selectedValue = value;
    this.valueChanged.emit(value);
  }

  setSelectedValue(value: string | null) {
    this.selectedValue = value;
  }
}
