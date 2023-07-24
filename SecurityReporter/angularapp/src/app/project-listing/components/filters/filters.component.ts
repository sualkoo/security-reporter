import { Component } from '@angular/core';
import { InputComponentComponent } from '../../../project-management/components/input-component/input-component.component';
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { FiltersDatepickerComponent } from '../datepicker/datepicker.component';
import { SliderComponent } from '../slider/slider.component';
import { SelectInterface } from '../../../project-management/interfaces/select-interface';
import { ProjectData, QuestionareIndex, projectScopeIndex, projectStatusIndex } from '../../interfaces/project-data';
import { filter } from 'rxjs';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  standalone: true,
  imports: [InputComponentComponent, SelectComponentComponent, FiltersDatepickerComponent, SliderComponent]
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
      case 'PNS':
        this.filteredClass.PentestStart = value;
        break;
      case 'PNE':
        this.filteredClass.PentestEnd = value;
        break;
      case 'PN':
        this.filteredClass.ProjectName = value;
        break;
    }
    console.log(this.filteredClass)
  }

  onChildDateValueChanged(value: Date, id: string) {
    if (id == 'STR') {
      this.filteredClass.StartDate = value;
    } else if (id == 'END') {
      this.filteredClass.EndDate = value;
    }
    console.log(this.filteredClass)
  }
}
