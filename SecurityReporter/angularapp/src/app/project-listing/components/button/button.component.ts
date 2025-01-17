import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'button-component',
  templateUrl: 'button.component.html',
  styleUrls: ['button.component.css'],
  standalone: true,
  imports: [MatButtonModule, MatDividerModule, MatIconModule],
})
export class ButtonComponent { }
