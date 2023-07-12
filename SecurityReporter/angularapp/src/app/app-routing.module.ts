import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {ProjectSearchComponent} from "./project-search/project-search.component";

const routes: Routes = [
 //{ path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '', component: ProjectSearchComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
