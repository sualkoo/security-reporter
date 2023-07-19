import { Component } from '@angular/core';
import {ProjectDataReport} from "../../interfaces/project-data-report.models";
import {ProjectDataService} from "../../providers/project-data-service";
import {NotificationService} from "../../providers/notification.service";

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.css', '../../project-search.css']
})
export class SearchbarComponent {
  constructor(private projectDataService: ProjectDataService, private notificationService: NotificationService) {
  }


  searchQuery: string | undefined;
  filters: string[] = ['Project name', 'Details', 'Refferences', 'Impact', 'CWE', 'Repeatability'];
  selectedFilters: { [key: string]: boolean } = {};
  isLoading: boolean = false;

  search(): void {
    this.isLoading = true;
    setTimeout(() => {
      // After the operation completes, reset isLoading to false
      this.isLoading = false;
    }, 2000);
    console.log('Performing search for:', this.searchQuery);
    this.searchQuery = '';
  }



//ID SERACH
  searchText: string = '';

  projectDataReport: ProjectDataReport = {
    id: '',
    documentInfo: undefined,
    executiveSummary: '',
    projectInfo: undefined,
    findings: [],
    scopeAndProcedures: undefined,
    testingMethodology: undefined
  };

  searchID() {

    this.projectDataService.getProjectReport(this.searchText).subscribe(
      (response) => {
        this.projectDataReport = response as ProjectDataReport;
        console.log(response);
      }
    )
  }


  subcategory: string = '';
  value: string = '';
  keyword: string = '';

  projectDataReports: ProjectDataReport[] = [];

  searchProjectReports() {

    this.isLoading = true;
    setTimeout(() => {
      // After the operation completes, reset isLoading to false
      this.isLoading = false;
    }, 2000);
    console.log('Performing search for:', this.value);


    this.projectDataService.getProjectReports(this.subcategory, this.keyword, this.value).subscribe(
      (response) => {
        this.projectDataReports = response as ProjectDataReport[];
        console.log(response);
        this.notificationService.displayMessage("Search completed", "success");
      }
    )
  }

  keywords = [
    { id: 'case1', value: 'ProjectReportName', label: 'Project Name' },
    { id: 'case2', value: 'SubsectionDetails', label: 'Details' },
    { id: 'case3', value: 'SubsectionImpact', label: 'Impact' },
    { id: 'case4', value: 'SubsectionRepeatability', label: 'Repeatability' },
    { id: 'case5', value: 'SubsectionReferences', label: 'References' },
    { id: 'case6', value: 'CWE', label: 'CWE' }
  ];


  isFormValid: boolean = false;

  checkFormValidity(): void {
    switch (this.keyword) {
      case 'ProjectReportName':
        this.subcategory = 'DocumentInfo';
        break;
      case 'SubsectionDetails':
        this.subcategory = 'Finding';
        break;
      case 'SubsectionImpact':
        this.subcategory = 'Finding';
        break;
      case 'SubsectionRepeatability':
        this.subcategory = 'Finding';
        break;
      case 'SubsectionReferences':
        this.subcategory = 'Finding';
        break;
      case 'CWE':
        this.subcategory = 'Finding';
        break;
      default: this.subcategory = '';
    }

    this.isFormValid = this.value.trim() !== '' && this.subcategory !== '' && this.keyword !== '';
  }
}
