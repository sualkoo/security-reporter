import { AfterViewInit, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { ProjectReportService } from '../../providers/project-report-service';
import { ProjectDataReport } from '../../interfaces/project-data-report.model';
import { NotificationService } from '../../providers/notification.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { delay, fromEvent, throttle, throttleTime } from 'rxjs';

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

  loadedReports: ProjectDataReport[] = []
  nextPage: string | undefined | null;
  lastLoadedPage: number = 1;
  isLoading: boolean = false;
  isLoadingNextPage: boolean = false;


  onSearch() {
    this.resetSearch();
    this.loadReports();
  }

  resetSearch() {
    const container = this.reportsScrollableBox.nativeElement;
    container.scrollTop = 0;
    this.loadedReports = [];
    this.nextPage = null;
    this.lastLoadedPage = 1;
  }

  displayNewReport(newReport: ProjectDataReport) {
    console.log("Displaying new report");
    this.loadedReports.push(newReport);
  }

  loadReports() {

      this.isLoading = true;
      this.projectReportService.getProjectReports(
        this.subcategory,
        this.keyword,
        this.value,
        1
      ).subscribe(
        (response) => {
          if (response.data.length == 0) {
            this.notificationService.displayMessage("No reports found.", "info");
          } else {
            this.loadedReports = response.data as ProjectDataReport[];
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
      this.subcategory,
      this.keyword,
      this.value,
      (this.lastLoadedPage + 1)
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
