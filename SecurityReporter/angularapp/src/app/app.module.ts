import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AppComponent } from './app.component';
import { AddProjectComponent } from './project-management/component-pages/add-project-page/add-project.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { NgFor } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { DatepickerComponent } from 'src/app/project-management/components/datepicker-component/datepicker-component.component';
import { SelectComponentComponent } from './project-management/components/select-component/select-component.component';
import { InputComponentComponent } from './project-management/components/input-component/input-component.component';
import { RadioButtonComponentComponent } from './project-management/components/radio-button-component/radio-button-component.component';

import { AppRoutingModule } from './app-routing.module';




@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    MatSlideToggleModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgFor,
    MatSelectModule,
    MatRadioModule,
    AddProjectComponent,
    DatepickerComponent,
    SelectComponentComponent,
    InputComponentComponent,
    RadioButtonComponentComponent,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
