import { Component, Input } from '@angular/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-input-component',
  templateUrl: './input-component.component.html',
  standalone: true,
  styleUrls: ['./input-component.component.css'],
  imports: [MatFormFieldModule, MatInputModule]
})
export class InputComponentComponent {
  @Input() title : string = "";
}
