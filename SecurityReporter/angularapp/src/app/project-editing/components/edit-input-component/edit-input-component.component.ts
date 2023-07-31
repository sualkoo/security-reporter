import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import {
  FormsModule,
  FormControl,
  FormGroupDirective,
  NgForm,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { FormGroup, FormBuilder, AsyncValidatorFn } from '@angular/forms';


@Component({
  selector: 'app-edit-input-component',
  templateUrl: './edit-input-component.component.html',
  styleUrls: ['./edit-input-component.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class EditInputComponentComponent {
  form: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      projectName: [
        '',
        Validators.compose([
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50),
          Validators.pattern(/^[A-Z].*$/),
        ]),
      ],
    });
    this.form.markAllAsTouched();
  }

  @Input() title: string = '';
  @Input() inputValue: string = '';
  @Output() valueChanged = new EventEmitter<string>();

  onInputChange() {
    this.valueChanged.emit(this.inputValue);
  }
}
