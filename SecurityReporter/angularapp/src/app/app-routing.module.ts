import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AddProjectComponent } from './project-management/component-pages/add-project-page/add-project.component';
import { ProjectSearchPageComponent } from './project-search/component-pages/project-search-page/project-search-page.component';
import { ListProjectsPageComponent } from './project-listing/component-pages/list-projects-page/list-projects-page.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { ProjectEditingPageComponent } from './project-editing/component-pages/project-editing-page/project-editing-page.component';
import { Roles } from './roles/roles';
import { LoginPageComponent } from './login-page/login-page.component';
import { DefaultPageComponentComponent } from './default-page/component-pages/default-page-component.component';
import { AfterLoginPageComponent } from './after-login-page/after-login-page.component';
import { loginGuard } from './login-page/guards/login.guard';
import { MyProfileComponent } from './my-profile/component-pages/my-profile-page/my-profile.component';

import { DashboardComponent } from './dashboard/dashboard.component';
import { AboutPentestsComponent } from './about-pentests/about-pentests.component';

const routes: Routes = [
  { path: '', redirectTo: 'welcome', pathMatch: 'full' },
  { path: 'welcome', component: LandingPageComponent, canActivate: [Roles], data: { allowedRoles: ["admin", "pentester", "coordinator", "client", "default"] } },
  { path: 'project-search', component: ProjectSearchPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'pentester'] } },
  { path: 'project-management', component: AddProjectComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'add-project', component: AddProjectComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'list-projects', component: ListProjectsPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator', 'client'] } },
  { path: 'edit-project/:id', component: ProjectEditingPageComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'dashboard', component: DashboardComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'coordinator'] } },
  { path: 'about-pentests', component: AboutPentestsComponent, canActivate: [Roles], data: { allowedRoles: ['admin', 'pentester', 'coordinator', 'client', 'default'] } },
  { path: 'log-in', component: LoginPageComponent , canActivate: [loginGuard]},
  { path: 'after-login', component: AfterLoginPageComponent, canActivate: [Roles], data: { allowedRoles: ["admin", "pentester", "coordinator", "client", "default"] } },
  { path: 'default-page', component: DefaultPageComponentComponent, canActivate: [Roles], data: { allowedRoles: ['default'] } },
  { path: 'my-profile', component: MyProfileComponent, canActivate: [Roles], data: { allowedRoles: ["admin", "pentester", "coordinator", "client"] } },
];
@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes), CommonModule],
  exports: [RouterModule],
})
export class AppRoutingModule {}
