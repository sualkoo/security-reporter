import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddProjectComponent } from './project-management/component-pages/add-project-page/add-project.component';


const routes: Routes = [
  { path: 'project-search', component: AddProjectComponent },
  { path: 'project-management', component: AddProjectComponent },
  { path: 'add-project', component: AddProjectComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes), CommonModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
