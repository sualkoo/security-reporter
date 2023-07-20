import {Component, EventEmitter, HostListener, Output} from '@angular/core';
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

  isLoading: boolean = false;

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: Event): void {
    const scrollY = window.scrollY || window.pageYOffset;
    const windowHeight = window.innerHeight;
    const documentHeight = document.body.scrollHeight;

    // Calculate the distance from the bottom of the page
    const distanceFromBottom = documentHeight - (scrollY + windowHeight);

    // Threshold to trigger the method (adjust as needed)
    const scrollThreshold = 100;

    // Call the method when the user scrolls to the bottom (or near the bottom)
    if (distanceFromBottom <= scrollThreshold) {
      if (this.totalPages > this.page) {
        this.page++;
        this.sendSearchRequest();
      }
    }
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
  page: number = 1;

  projectDataReports: ProjectDataReport[] = [];
  @Output() reports = new EventEmitter<ProjectDataReport[]>()
  totalPages: number = 0;

  searchProjectReports() {
   this.page =1;
   this.projectDataReports = [];
    this.isLoading = true;

    console.log('Performing search for:', this.value);
    this.sendSearchRequest();
    this.notificationService.displayMessage("Search completed", "success");
  }

  sendSearchRequest() {
    this.projectDataService.getProjectReports(this.subcategory, this.keyword, this.value, this.page).subscribe(
      (response: any) => {
        const responseData = response as {
          pageNumber: number;
          totalPages: number;
          data: ProjectDataReport[];
        };

        this.totalPages = responseData.totalPages;
        this.isLoading = false;

        this.projectDataReports = this.projectDataReports.concat(responseData.data);
        this.reports.emit(this.projectDataReports);

        //console.log(this.projectDataReports);

        if (this.page < this.totalPages) {
          console.log(this.page)
          console.log(this.totalPages)
        }

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
