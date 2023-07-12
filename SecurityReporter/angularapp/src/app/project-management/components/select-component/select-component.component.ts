import { Component, Input } from '@angular/core';
import {NgFor} from '@angular/common';
import {MatSelectModule} from '@angular/material/select';
import { SelectInterface } from '../../interfaces/select-interface';

@Component({
  selector: 'app-select-component',
  templateUrl: './select-component.component.html',
  styleUrls: ['./select-component.component.css'],
  standalone: true,
  imports: [NgFor, MatSelectModule,]
})

export class SelectComponentComponent {  
  @Input() option : SelectInterface[] = [];
  @Input() title : string = "";
}
