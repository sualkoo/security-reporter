import { Component, ElementRef, HostListener, OnInit, Renderer2, ViewChild } from '@angular/core';
import { ProjectReportService } from '../../providers/project-report-service';
import { NotificationService } from '../../providers/notification.service';
import { fromEvent } from 'rxjs';
import { FindingResponse } from '../../interfaces/finding-response.model';
import { HttpErrorResponse } from '@angular/common/http';
import { GroupedFinding } from '../../interfaces/grouped-findings.model';

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search-page.component.html',
  styleUrls: ['./project-search-page.component.css', '../../../project-search.css'],
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
    const atBottom = container.scrollTop + container.clientHeight + 50 >= container.scrollHeight;
    return atBottom;
  }

  loadedFindings: FindingResponse[] = []
  groupedFindings: GroupedFinding[] = [];

  totalRecords?: number;
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
    this.groupedFindings = [];
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

  injectionOfSendingVariables() {
    this.projectNameSending = this.projectName;
    this.detailsSending = this.details;
    this.impactSending = this.impact;
    this.repeatabilitySending = this.repeatability;
    this.referencesSending = this.references;
    this.cweSending = this.cwe;
  }

  groupFindings() {
    const groupedFindingsMap = new Map<string, GroupedFinding>();

    for (const findingRes of this.loadedFindings) {
      const { projectReportId, projectReportName, finding } = findingRes;

      if (groupedFindingsMap.has(projectReportId)) {
        // Add the finding to an existing group
        const existingGroup = groupedFindingsMap.get(projectReportId);
        existingGroup?.findings.push(finding);
      } else {
        // Create a new group
        const isProjectSelected = this.groupedFindings.some(gf => gf.projectId === projectReportId && gf.checked === true);
        const newGroup: GroupedFinding = {
          projectId: projectReportId,
          projectName: projectReportName,
          findings: [finding],
          checked: isProjectSelected
        };
        groupedFindingsMap.set(projectReportId, newGroup);
      }
    }

    // Convert the map to an array of GroupedFinding objects
    this.groupedFindings = Array.from(groupedFindingsMap.values());
  }

  loadFindings() {
    this.highlightValue = this.value;
    this.injectionOfTestVariables();
    this.isLoading = true;
    this.projectReportService.getProjectReportFindings(
      1,
      this.projectName,
      this.details,
      this.impact,
      this.repeatability,
      this.references,
      this.cwe
    ).subscribe(
      (response) => {
        this.injectionOfSendingVariables();
        if (response.data.length == 0) {
          this.notificationService.displayMessage("No findings found.", "info");
        } else {
          this.loadedFindings = response.data;
          this.groupFindings();
          this.totalRecords = response.totalRecords;
          this.nextPage = response.nextPage;
          this.lastLoadedPage = response.pageNumber;
        }
        this.isLoading = false;
      }, (HttpErrorResponse) => {
        this.isLoading = false;
        this.notificationService.displayMessage("No findings found.", "info");
      }
    )
  }

  // Scrollable window
  @ViewChild('reportsScrollableBox', { static: true }) reportsScrollableBox!: ElementRef;

  loadNextPage() {
    this.isLoadingNextPage = true;
    this.projectReportService.getProjectReportFindings(
      (this.lastLoadedPage + 1),
      this.projectNameSending,
      this.detailsSending,
      this.impactSending,
      this.repeatabilitySending,
      this.referencesSending,
      this.cweSending
    ).subscribe(res => {
      this.lastLoadedPage = res.pageNumber;
      this.nextPage = res.nextPage;
      for (let report of res.data) {
        this.loadedFindings.push(report);
      }
      this.groupFindings();
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

  projectNameSending?: string;
  detailsSending?: string;
  impactSending?: string;
  repeatabilitySending?: string;
  referencesSending?: string;
  cweSending?: string;

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

    for(let value of this.keywordsValues) {
      if (value == 'ProjectReportName') {
        this.projectName = this.value;
     }
      else if(value == 'SubsectionDetails') {
        this.details = this.value;
      }
      else if(value == 'SubsectionImpact') {
        this.impact = this.value;
      }
      else if(value == 'SubsectionRepeatability') {
        this.repeatability = this.value;
      }
      else if(value == 'SubsectionReferences') {
        this.references = this.value;
      }
      else if(value == 'CWE') {
        this.cwe = this.value;
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

      this.notificationService.displayMessage("If you choose CWE, search value has to be a number", "warning");
    }
  }

  onGetSource(projectId: string): void {
    this.notificationService.displayMessage("Feature in development.", "info");
  }

  onSelectionCheckboxChange(event: Event) {
    const target = event.target as HTMLInputElement;

    if (target.checked) {
      this.groupedFindings.find(p => p.projectId == target.value)!.checked = true;
    } else {
      this.groupedFindings.find(p => p.projectId == target.value)!.checked = false;
    }

    this.selectedProjects = [];
    this.groupedFindings.forEach(gf => {
      if (gf.checked) {
        this.selectedProjects.push(gf );
      }
    })
  }

  selectedProjects: GroupedFinding[] = [];

  onDeleteSelectedProjects() {
    const projectIds: string[] = this.selectedProjects.map(p => p.projectId);
    // BE request
    this.projectReportService.deleteProjectReport(projectIds).subscribe((res) => {
      // Success
      for (let selectedGf of this.selectedProjects) {
        this.loadedFindings = this.loadedFindings.filter(lf => lf.projectReportId !== selectedGf.projectId);
        this.groupedFindings = this.groupedFindings.filter(gf => gf.projectId !== selectedGf.projectId);
        this.totalRecords = this.totalRecords! - selectedGf.findings.length;
      }
      this.groupFindings();
      this.selectedProjects = [];
      this.notificationService.displayMessage("Successfully removed from db!", "info");
    }, (e: HttpErrorResponse) => {
      // Error
      console.error(e);
      this.selectedProjects = [];
    })

    
  }

  showPopup: boolean = false;

  openPopup() {
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }

  showSidebar: boolean = true;

  toggleSidebar() {
    this.showSidebar = !this.showSidebar;
  }

}
