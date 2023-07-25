import { Component } from '@angular/core';
import { InputComponentComponent } from '../../../project-management/components/input-component/input-component.component';
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { FiltersDatepickerComponent } from '../datepicker/datepicker.component';
import { SliderComponent } from '../slider/slider.component';
import { SelectInterface } from '../../../project-management/interfaces/select-interface';
import { ProjectData, QuestionareIndex, projectScopeIndex, projectStatusIndex } from '../../interfaces/project-data';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  standalone: true,
  imports: [InputComponentComponent, SelectComponentComponent, FiltersDatepickerComponent, SliderComponent, FormsModule]
})
export class FiltersComponent {

  ProjectStatus: SelectInterface[] = [
    { value: 'Requested', viewValue: 'Requested' },
    { value: 'Planned', viewValue: 'Planned' },
    { value: 'In progress', viewValue: 'In progress' },
    { value: 'Finished', viewValue: 'Finished' },
    { value: 'Cancelled', viewValue: 'Cancelled' },
    { value: 'On hold', viewValue: 'On hold' },
  ];

  ProjectScope: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'Sent', viewValue: 'Sent' },
    { value: 'Confirmed', viewValue: 'Confirmed' },
    { value: 'Signed', viewValue: 'Signed' },
  ];

  Questionare: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'Sent', viewValue: 'Sent' },
    { value: 'Received', viewValue: 'Received' },
  ];

  IKO: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'IKO', viewValue: 'IKO' },
  ];

  TKO: SelectInterface[] = [
    { value: 'TBS', viewValue: 'TBS' },
    { value: 'TKO', viewValue: 'TKO' },
  ];

  filteredClass: ProjectData = {}
  sliderValues: number[] = [2,10];

  onChildInputValueChanged(value: string, id: string) {
    switch (id) {
      case 'PST':
        //@ts-ignore
        this.filteredClass.ProjectStatus = projectStatusIndex[value];
        break;
      case 'QUE':
        //@ts-ignore
        this.filteredClass.Questionare = QuestionareIndex[value];
        break;
      case 'PSC':
        //@ts-ignore
        this.filteredClass.ProjectScope = projectScopeIndex[value];
        break;
      case 'PN':
        this.filteredClass.ProjectName = value;
        break;
    }
  }

  onChildDateValueChanged(value: Date, id: string) {
    if (id == 'STR') {
      this.filteredClass.StartDate = value;
    } else {
      this.filteredClass.EndDate = value;
    }
  }

  onChildSliderValueChanged(event: { start: number, end: number }) {    
    this.filteredClass.PentestStart = event.start;
    this.filteredClass.PentestEnd = event.end;
  }
}
