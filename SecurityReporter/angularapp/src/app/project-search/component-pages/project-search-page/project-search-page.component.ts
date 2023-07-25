import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ProjectReportService } from '../../providers/project-report-service';
import { ProjectDataReport } from '../../interfaces/project-data-report.model';
import { NotificationService } from '../../providers/notification.service';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search-page.component.html',
  styleUrls: ['./project-search-page.component.css', '../../project-search.css'],
})
export class ProjectSearchPageComponent implements OnInit {
  constructor(private projectReportService: ProjectReportService, private notificationService: NotificationService) { }

  ngOnInit(): void {
    // Use the RxJS fromEvent operator to handle the scroll event
    fromEvent(this.reportsScrollableBox.nativeElement, 'scroll')
      .subscribe(() => {
        if (this.isScrolledToBottom()) {
          if (this.nextPage && !this.isLoadingNextPage) {
            this.loadNextPage();
          }
        }
      });;
  }

  isScrolledToBottom(): boolean {
    const container = this.reportsScrollableBox.nativeElement;
    const atBottom = container.scrollTop + container.clientHeight + 10 >= container.scrollHeight;
    return atBottom;
  }

  totalRecords?: number;
  newReport?: ProjectDataReport;
  loadedReports: ProjectDataReport[] = []
  nextPage: string | undefined | null;
  lastLoadedPage: number = 1;
  isLoading: boolean = false;
  isLoadingNextPage: boolean = false;


  onSearch() {
    console.log(this.keywordsValues);
    this.resetView();
    this.resetSearch();
    this.loadReports();
  }

  resetView() {
    const container = this.reportsScrollableBox.nativeElement;
    container.scrollTop = 0;
  }

  resetSearch() {

    this.loadedReports = [];
    this.totalRecords = 0;
    this.nextPage = null;
    this.lastLoadedPage = 1;
    this.newReport = undefined;
  }

  displayNewReport(newReport: ProjectDataReport) {
    this.resetView();
    console.log("Displaying new report");
    this.newReport = newReport;
  }

  loadReports() {

    this.isLoading = true;
   this.projectReportService.getProjectReports(
      this.value,
      1,
      this.projectName,
      this.details,
      this.impact,
      this.repeatability,
      this.references,
      this.cwe
    ).subscribe(
      (response) => {
        console.log(response)
        if (response.data.length == 0) {
          this.notificationService.displayMessage("No reports found.", "info");
        } else {
          this.loadedReports = response.data as ProjectDataReport[];
          this.totalRecords = response.totalRecords;
          this.nextPage = response.nextPage;
          this.lastLoadedPage = response.pageNumber;
        }
        this.isLoading = false;
      }
    )
  }

  // Scrollable window
  @ViewChild('reportsScrollableBox', { static: true }) reportsScrollableBox!: ElementRef;

  loadNextPage() {
    console.log("Loading next page")
    this.isLoadingNextPage = true;
    this.projectReportService.getProjectReports(
      this.value,
      (this.lastLoadedPage + 1),
      this.projectName,
      this.details,
      this.impact,
      this.repeatability,
      this.references,
      this.cwe
    ).subscribe(res => {
      this.lastLoadedPage = res.pageNumber;
      this.nextPage = res.nextPage;
      for (let report of res.data) {
        this.loadedReports.push(report);
      }
      this.isLoadingNextPage = false;
    })
  }

  value: string = '';
  keyword: string = '';
  subcategory: string = '';

  keywordsValues: string[] = [];

  keywords = [
    { id: 'case1', value: 'ProjectReportName', label: 'Project Name', checked: false},
    { id: 'case2', value: 'SubsectionDetails', label: 'Details', checked: false},
    { id: 'case3', value: 'SubsectionImpact', label: 'Impact', checked: false},
    { id: 'case4', value: 'SubsectionRepeatability', label: 'Repeatability', checked: false},
    { id: 'case5', value: 'SubsectionReferences', label: 'References', checked: false},
    { id: 'case6', value: 'CWE', label: 'CWE', checked: false }
  ];


  projectName?: string;
  details?: string;
  impact?: string;
  repeatability?: string;
  references?: string;
  cwe?: string;


  toggleCheckbox(selectedCase: any): void {
    this.keywords.forEach((caseItem) => {
      if (caseItem === selectedCase) {
        caseItem.checked = !caseItem.checked;
      }
    });

    if (selectedCase.checked) {
      this.keyword = selectedCase.value;
      this.keywordsValues.push(selectedCase.value);
      this.isCheckboxChecked = true;
    } else {
      this.keywordsValues = this.keywordsValues.filter(value => value !== selectedCase.value);
      if (this.keywordsValues.length == 0) {
        this.isCheckboxChecked = false;
      }
    }
    this.checkFormValidity();
  }

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

    this.isFormValid = this.value.trim() !== '' && this.subcategory !== '' && this.keywordsValues.length > 0;
    console.log(this.keywordsValues);
    for(let value of this.keywordsValues) {
     if(value == 'ProjectReportName') {
       this.projectName = value;
     }
      if(value == 'SubsectionDetails') {
        this.details = value;
      }
      if(value == 'SubsectionImpact') {
        this.impact = value;
      }
      if(value == 'SubsectionRepeatability') {
        this.repeatability = value;
      }
      if(value == 'SubsectionReferences') {
        this.references = value;
      }
      if(value == 'CWE') {
        this.cwe = value;
      }
    }
  }

  isCheckboxChecked: boolean = false;

  onFilterDelete() {
    this.keywords.forEach((caseItem) => {
        caseItem.checked =false;
        this.keywordsValues = [];
        this.isCheckboxChecked = false;
        this.isFormValid = false;
      }
    );

  }

}
