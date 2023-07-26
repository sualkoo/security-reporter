import { Component, EventEmitter, Output } from '@angular/core';
import { SelectComponentComponent } from '../../../project-management/components/select-component/select-component.component';
import { FiltersDatepickerComponent } from '../datepicker/datepicker.component';
import { SliderComponent } from '../slider/slider.component';
import { SelectInterface } from '../../../project-management/interfaces/select-interface';
import { IKOIndex, ProjectData, QuestionareIndex, projectScopeIndex, projectStatusIndex } from '../../interfaces/project-data';
import { FormsModule } from '@angular/forms';
import { InputComponent } from '../input/input.component';



@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  standalone: true,
  imports: [SelectComponentComponent, FiltersDatepickerComponent, SliderComponent, FormsModule, InputComponent]
})
export class FiltersComponent {

  constructor() {

  }

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
    { value: 'TBD', viewValue: 'TBD' },
    { value: 'Date is set', viewValue: 'Date is set' },
  ];

  TKO: SelectInterface[] = [
    { value: 'TBD', viewValue: 'TBD' },
    { value: 'Date is set', viewValue: 'Date is set' },
  ];

  filteredClass: ProjectData = {};
  url: string = '';
  

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
      case 'IKO':
        //@ts-ignore
        this.filteredClass.IKO = IKOIndex[value];
        break;
      case 'TKO':
        //@ts-ignore
        this.filteredClass.TKO = IKOIndex[value];
        break;
    }
    this.filtersChanged();
  }

  onChildDateValueChanged(value: Date, id: string) {
    if (id == 'STR') {
      this.filteredClass.StartDate = value;
    } else if (id == 'END'){
      this.filteredClass.EndDate = value;
    }
    console.log(this.filteredClass)
    this.filtersChanged();
  }

  onChildSliderValueChanged(event: { start: number, end: number }) {    
    this.filteredClass.PentestStart = event.start;
    this.filteredClass.PentestEnd = event.end;
    this.filtersChanged();
  }

  @Output() filtersChangedEvent = new EventEmitter<string>();

  filtersChanged() {
    this.url = this.convertProjectDataToQueryString(this.filteredClass) + '&year=0&month=0&day=0&dayOfWeek=0';
    this.filtersChangedEvent.emit(this.url);
  }

  convertProjectDataToQueryString(data: ProjectData): string {
  const queryStringParams: string[] = [];

    const encodeQueryParamValue = (value: string | number | boolean | Date): string => {
      if (value instanceof Date) {
        return encodeURIComponent(value.toISOString());
      }
      return encodeURIComponent(value.toString());
    };

    // Append each attribute and its value to the queryStringParams array
    if (data.ProjectStatus) {
      queryStringParams.push(`&FilteredProjectStatus=${encodeQueryParamValue(data.ProjectStatus)}`);
    }
    if (data.Questionare) {
      queryStringParams.push(`&FilteredProjectQuestionare=${encodeQueryParamValue(data.Questionare)}`);
    }
    if (data.ProjectScope) {
      queryStringParams.push(`&FilteredProjectScope=${encodeQueryParamValue(data.ProjectScope)}`);
    }
    if (data.ProjectName) {
      queryStringParams.push(`&FilteredProjectName=${encodeQueryParamValue(data.ProjectName)}`);
    }
    if (data.IKO) {
      queryStringParams.push(`&FilteredIKO=${encodeQueryParamValue(data.IKO)}`);
    }
    if (data.TKO) {
      queryStringParams.push(`&FilteredTKO=${encodeQueryParamValue(data.TKO)}`);
    }
    if (data.StartDate) {
      const formattedDate = data.StartDate.toISOString().split('T')[0];
      queryStringParams.push(`&FilteredStartDate=${formattedDate}`);
    }
    if (data.EndDate) {
      const formattedDate = data.EndDate.toISOString().split('T')[0];
      queryStringParams.push(`&FilteredEndDate=${formattedDate}`);
    }
    if (data.PentestStart !== undefined && data.PentestEnd !== undefined) {
      queryStringParams.push(`&FilteredPentestStart=${data.PentestStart}&FilteredPentestEnd=${data.PentestEnd}`);
    }
    return queryStringParams.join('');
  }
}
