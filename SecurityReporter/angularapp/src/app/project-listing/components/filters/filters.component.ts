import { Component } from '@angular/core';
import { InputComponentComponent } from '../../../project-management/components/input-component/input-component.component';
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { FiltersDatepickerComponent } from '../datepicker/datepicker.component';
import { SliderComponent } from '../slider/slider.component';



@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  standalone: true,
  imports: [InputComponentComponent, SelectComponentComponent, FiltersDatepickerComponent, SliderComponent]
})
export class FiltersComponent {

}
