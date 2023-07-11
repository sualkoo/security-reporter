import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AppComponent } from './app.component';
import { ProjectManagerComponent } from './project-manager/project-manager.component';
import { ProjectManagementComponent } from './bratislava/project-management/project-management.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatNativeDateModule} from '@angular/material/core';
import {NgFor} from '@angular/common';
import {MatSelectModule} from '@angular/material/select';
import {MatRadioModule} from '@angular/material/radio';
import {DatepickerComponent } from 'src/app/bratislava/components/datepicker-component/datepicker-component.component'
import { SelectComponentComponent } from './bratislava/components/select-component/select-component.component';
import { InputComponentComponent } from './bratislava/components/input-component/input-component.component';
import { RadioButtonComponentComponent } from './bratislava/components/radio-button-component/radio-button-component.component';

@NgModule({
  declarations: [
    AppComponent,
    ProjectManagerComponent,     
  ],
  imports: [
    BrowserModule, HttpClientModule, MatSlideToggleModule, BrowserAnimationsModule, MatFormFieldModule, MatInputModule, 
    MatDatepickerModule, MatNativeDateModule, NgFor, MatSelectModule, MatRadioModule, ProjectManagementComponent, DatepickerComponent, 
    SelectComponentComponent, InputComponentComponent, RadioButtonComponentComponent,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
