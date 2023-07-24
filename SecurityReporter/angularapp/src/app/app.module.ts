import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AppComponent } from './app.component';
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
import { ProjectSearchPageComponent } from './project-search/component-pages/project-search-page/project-search-page.component';
import { CommonModule } from '@angular/common';
import { ListProjectsPageComponent } from './project-listing/component-pages/list-projects-page/list-projects-page.component';
import { DataGridComponentComponent } from './project-listing/components/data-grid-component/data-grid-component.component';
import { MatButtonModule } from '@angular/material/button';
import { DeletePopupComponentComponent } from './project-listing/components/delete-popup-component/delete-popup-component.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSidenavModule } from '@angular/material/sidenav';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddProjectReportComponent } from './project-search/components/add-project-report/add-project-report.component';


@NgModule({
  declarations: [
    AppComponent,
    ProjectSearchPageComponent,
    AddProjectReportComponent
  ],
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
    DatepickerComponent,
    SelectComponentComponent,
    InputComponentComponent,
    RadioButtonComponentComponent,
    AppRoutingModule,
    MatSnackBarModule,
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
    MatSidenavModule,
    ScrollingModule,
    MatSnackBarModule,
    MatIconModule,
    CommonModule,
    ListProjectsPageComponent,
    DataGridComponentComponent,
    MatButtonModule,
    DeletePopupComponentComponent,
    MatDialogModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
