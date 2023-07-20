import { Component, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { ProjectDataReport } from '../../interfaces/project-data-report.models';

@Component({
  selector: 'app-project-search-results-page',
  templateUrl: './project-search-results-page.component.html',
  styleUrls: ['./project-search-results-page.component.css', '../../project-search.css']
})
export class ProjectSearchResultsPageComponent {
  searchKeyword: string | undefined;
  filters: string[] = ['Project name', 'Details', 'Refferences', 'Impact', 'CWE', 'Repeatability'];
  /*isSidebarOpen: boolean = false;*/

  text = 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.';
  projects: string[][] = [['Project 1', this.text], ['Project 2', this.text], ['Project 3', this.text], ['Project 4', this.text], ['Project 5', this.text], ['Project 6', this.text], ['Project 7', this.text]];
  projectReports: ProjectDataReport[] = [];

  @ViewChild('sidenav')
  sidenav!: MatSidenav;

  public toggleSidenav(): void {
    this.sidenav.toggle();
  }

  logData(value: any) {
    this.projectReports = value;
  }

  search(): void {
    // TODO search functionality
  
  }

  getSource(): void {
    // TODO getSource functionality

  }

  addProject(): void {
    // TODO addProject functionality

  }

}
