import { AfterViewInit, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { ProjectReportService } from '../../providers/project-report-service';
import { ProjectDataReport } from '../../interfaces/project-data-report.model';
import { NotificationService } from '../../providers/notification.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { delay } from 'rxjs';

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search-page.component.html',
  styleUrls: ['./project-search-page.component.css', '../../project-search.css'],
})
export class ProjectSearchPageComponent {
  constructor(private projectReportService: ProjectReportService, private notificationService: NotificationService) { }

  form = new FormGroup({
    subcategory: new FormControl("DocumentInfo", Validators.required),
    keyword: new FormControl("ProjectReportName", Validators.required),
    value: new FormControl("Du", Validators.required)
  })

  loadedReports: ProjectDataReport[] = []
  nextPage: string | undefined | null;
  lastLoadedPage: number = 1;
  isLoading: boolean = false;
  isLoadingNextPage: boolean = false;


  onSearch() {
    this.loadReports();
  }

  loadReports() {
    if (this.form.controls['value'].value?.length !== 0) {

      this.isLoading = true;
      this.projectReportService.getProjectReports(
        this.form.controls['subcategory'].value as string,
        this.form.controls['keyword'].value as string,
        this.form.controls['value'].value as string,
        1
      ).pipe(delay(500)).subscribe(
        (response) => {
          console.log(response)
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
    } else {
      this.notificationService.displayMessage("Value not specified.", "info")
    }
  }

  // Scrollable window
  @ViewChild('reportsScrollableBox', { static: true }) reportsScrollableBox!: ElementRef;
  @HostListener('scroll', ['$event'])
  onScroll() {
    const container = this.reportsScrollableBox!.nativeElement;

    // Calculate the scroll position and check if it's at the bottom
    const atBottom = container.scrollTop + container.clientHeight === container.scrollHeight;

    if (atBottom) {
      this.loadNextPage();
    }
  }

  loadNextPage() {
    if (this.nextPage) {
      console.log("Loading next page")
      this.isLoadingNextPage = true;
      this.projectReportService.getProjectReports(
        this.form.controls['subcategory'].value as string,
        this.form.controls['keyword'].value as string,
        this.form.controls['value'].value as string,
        (this.lastLoadedPage + 1)
      ).subscribe(res => {
        this.lastLoadedPage = res.pageNumber;
        this.nextPage = res.nextPage;
        for (let report of res.data) {
          this.loadedReports.push(report);
        }
        this.isLoadingNextPage = false;
      })

    } else {
      console.log("Last page loaded")
    }
  }
}
