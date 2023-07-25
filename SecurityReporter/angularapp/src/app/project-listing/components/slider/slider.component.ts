import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatSliderModule } from '@angular/material/slider';



@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css'],
  standalone: true,
  imports: [MatSliderModule, FormsModule],
})
export class SliderComponent {
  
  startValue: number = 2;
  endValue: number = 10;

  @Output() valuesChanged = new EventEmitter<{ start: number, end: number }>();

  onSliderChange() {
    this.valuesChanged.emit({ start: this.startValue, end: this.endValue });
  }

}
