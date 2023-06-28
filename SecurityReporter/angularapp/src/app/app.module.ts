import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { AppComponent } from './app.component';
import { ProjectManagerComponent } from './project-manager/project-manager.component';

@NgModule({
  declarations: [
    AppComponent,
    ProjectManagerComponent
  ],
  imports: [
    BrowserModule, HttpClientModule, MatSlideToggleModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
