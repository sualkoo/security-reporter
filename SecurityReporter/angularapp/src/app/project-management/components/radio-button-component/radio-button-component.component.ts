import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';
import { ProjectInterface } from '../../interfaces/project-interface';

@Component({
  selector: 'app-radio-button-component',
  templateUrl: './radio-button-component.component.html',
  styleUrls: ['./radio-button-component.component.css'],
  standalone: true,
  imports: [MatRadioModule, FormsModule],
})
export class RadioButtonComponentComponent implements OnInit {
  @Input() title: string = '';
  @Input() project: ProjectInterface | undefined = undefined;
  radioValue: string = '0';
  inputValue: number | undefined = 5;

  @Output() valueChanged = new EventEmitter<number>();

  async ngOnInit() {
    await this.waitForProjectName();

    if (this.project) {
      if (this.project.PentestDuration == 2) {
        this.radioValue = '2';
      } else if (this.project.PentestDuration == 4) {
        this.radioValue = '4';
      } else {
        this.radioValue = '3';
        this.inputValue = this.project.PentestDuration;
      }
    }
  }

  waitForProjectName(): Promise<void> {
    return new Promise<void>((resolve) => {
      const checkProjectName = () => {
        if (this.project && this.project.ProjectName !== '') {
          resolve();
        } else {
          setTimeout(checkProjectName, 100);
        }
      };

      checkProjectName();
    });
  }

  onRadioChange() {
    if (parseInt(this.radioValue) == 3) {
      this.valueChanged.emit(this.inputValue);
    } else {
      this.valueChanged.emit(parseInt(this.radioValue));
    }
  }
}
