import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ProjectReportService } from '../../providers/project-report-service';
import { ProjectReport } from '../../interfaces/project-report.model';
import { NotificationService } from '../../providers/notification.service';
import { fromEvent } from 'rxjs';
import { FindingResponse } from '../../interfaces/finding-response.model';

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
  loadedFindings: FindingResponse[] = []
  nextPage: string | undefined | null;
  lastLoadedPage: number = 1;
  isLoading: boolean = false;
  isLoadingNextPage: boolean = false;


  onSearch() {
    console.log(this.keywordsValues);
    this.resetView();
    this.resetSearch();
    this.loadFindings();
  }

  resetView() {
    const container = this.reportsScrollableBox.nativeElement;
    container.scrollTop = 0;
  }

  resetSearch() {
    this.loadedFindings = [];
    this.totalRecords = 0;
    this.nextPage = null;
    this.lastLoadedPage = 1;
  }

  injectionOfTestVariables() {
    this.projectNameTest = this.projectName;
    this.detailsTest = this.details;
    this.impactTest = this.impact;
    this.repeatabilityTest = this.repeatability;
    this.referencesTest = this.references;
    this.cweTest = this.cwe;
  }

  loadFindings() {
    this.highlightValue = this.value;
    this.injectionOfTestVariables();
    this.isLoading = true;
   this.projectReportService.getProjectReportFindings(
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
          this.notificationService.displayMessage("No findings found.", "info");
        } else {
          this.loadedFindings = response.data;
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
    this.projectReportService.getProjectReportFindings(
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
        this.loadedFindings.push(report);
      }
      this.isLoadingNextPage = false;
    })
  }

  value: string = '';
  keyword: string = '';
  highlightValue: string = '';
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

  projectNameTest?: string;
  detailsTest?: string;
  impactTest?: string;
  repeatabilityTest?: string;
  referencesTest?: string;
  cweTest?: string;

  clearReportVariables() {
    this.projectName = '';
    this.details = '';
    this.impact = '';
    this.repeatability = '';
    this.references = '';
    this.cwe = '';
  }


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
    this.clearReportVariables();
    this.checkFormValidity();
  }

  isFormValid: boolean = false;

  checkFormValidity(): void {

    this.isFormValid = this.value.trim() !== '' && this.keywordsValues.length > 0;


    console.log(this.keywordsValues);
    for(let value of this.keywordsValues) {
      if(value == 'ProjectReportName') {
       this.projectName = value;
     }
      else if(value == 'SubsectionDetails') {
        this.details = value;
      }
      else if(value == 'SubsectionImpact') {
        this.impact = value;
      }
      else if(value == 'SubsectionRepeatability') {
        this.repeatability = value;
      }
      else if(value == 'SubsectionReferences') {
        this.references = value;
      }
      else if(value == 'CWE') {
        this.cwe = value;
        if(this.value.length > 0)
        this.onlyNumbers(this.value);
      }
    }
  }

  isCheckboxChecked: boolean = false;

  onFilterDelete() {
    this.keywords.forEach((caseItem) => {
        caseItem.checked =false;
        this.keywordsValues = [];
        this.isCheckboxChecked = false;
      }
    );
    this.isFormValid = false;
    this.clearReportVariables();
  }


  onlyNumbers(event: string, ): void {
    const numberRegex = /^[0-9]+$/;
    console.log(numberRegex.test(event));

    if (!numberRegex.test(event)) {
      this.cwe = '';
      const index = this.keywordsValues.indexOf('CWE');
      if (index !== -1) {
        this.keywordsValues.splice(index, 1);
      }
      this.keywords.forEach((caseItem) => {

        if (caseItem.value === 'CWE') {
         //delay 200ms
          setTimeout(() => {
          caseItem.checked = false;
        }, 100);
        }
      });

      this.notificationService.displayMessage("If you choose CWE search value has to be a number", "warning");
    }
  }


}
