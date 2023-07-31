import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddProjectComponent } from './project-management/component-pages/add-project-page/add-project.component';
import { ProjectSearchPageComponent } from './project-search/component-pages/project-search-page/project-search-page.component';
import { ListProjectsPageComponent } from './project-listing/component-pages/list-projects-page/list-projects-page.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ProjectEditingPageComponent } from './project-editing/component-pages/project-editing-page/project-editing-page.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'project-search', component: ProjectSearchPageComponent },
  { path: 'project-management', component: AddProjectComponent },
  { path: 'add-project', component: AddProjectComponent },
  { path: 'list-projects', component: ListProjectsPageComponent },
  { path: 'edit-project', component: ProjectEditingPageComponent },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes), CommonModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
