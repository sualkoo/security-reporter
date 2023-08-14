import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddProjectComponent } from './project-management/component-pages/add-project-page/add-project.component';
import { ProjectSearchPageComponent } from './project-search/component-pages/project-search-page/project-search-page.component';
import { ListProjectsPageComponent } from './project-listing/component-pages/list-projects-page/list-projects-page.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ProjectEditingPageComponent } from './project-editing/component-pages/project-editing-page/project-editing-page.component';
import { Roles } from './roles/roles';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AboutPentestsComponent } from './about-pentests/about-pentests.component';

const routes: Routes = [
  { path: '', component: LandingPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'pentester', 'coordinator', 'client', 'default'] } },
  { path: 'project-search', component: ProjectSearchPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'pentester'] } },
  { path: 'project-management', component: AddProjectComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'add-project', component: AddProjectComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'list-projects', component: ListProjectsPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator', 'client'] } },
  { path: 'edit-project/:id', component: ProjectEditingPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'dashboard', component: DashboardComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'about-pentests', component: AboutPentestsComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'pentester', 'coordinator', 'client', 'default'] } },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes), CommonModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
