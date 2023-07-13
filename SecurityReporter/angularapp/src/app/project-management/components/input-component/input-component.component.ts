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

@Component({
  selector: 'app-input-component',
  templateUrl: './input-component.component.html',
  standalone: true,
  styleUrls: ['./input-component.component.css'],
  imports: [
    MatFormFieldModule,
    MatInputModule,
    NgIf,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class InputComponentComponent {
  @Input() title: string = '';
  @Input() inputValue: string = '';
  @Output() valueChanged = new EventEmitter<string>();

  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.minLength(3),
    Validators.maxLength(50),
    Validators.pattern(/^[A-Z].*$/),
  ]);

  matcher = new MyErrorStateMatcher();

  onInputChange() {
    this.valueChanged.emit(this.inputValue);
  }
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSubmitted = form && form.submitted;
    return !!(
      control &&
      control.invalid &&
      (control.dirty || control.touched || isSubmitted)
    );
  }
}

export class InputErrorStateMatcherExample {
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  matcher = new MyErrorStateMatcher();
}
