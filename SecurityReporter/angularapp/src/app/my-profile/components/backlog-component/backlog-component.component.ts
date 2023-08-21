import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { GetRoleService } from '../../../shared/services/get-role.service';
import { FiltersComponent } from '../../../project-listing/components/filters/filters.component';
import { ExpansionPanelComponent } from '../../../project-listing/components/expansion-panel/expansion-panel.component';
import { DeletePopupComponentComponent } from '../../../project-listing/components/delete-popup-component/delete-popup-component.component';
import { GetProjectsCountService } from '../../../project-listing/services/get-projects-count.service';
import { GetProjectsServiceService } from '../../../project-listing/services/get-projects-service.service';


@Component({
  selector: 'app-backlog-component',
  templateUrl: './backlog-component.component.html',
  styleUrls: ['./backlog-component.component.css'],
  standalone: true,
  imports: [MatTableModule, MatCheckboxModule, MatPaginatorModule, MatProgressSpinnerModule,
    CommonModule, MatButtonModule, MatTooltipModule, MatIconModule, FiltersComponent, ExpansionPanelComponent],
})
export class BacklogComponentComponent implements AfterViewInit {
  projects: ProjectInterface[] = [];
  checkedRows: Set<any> = new Set<any>();
  isLoading = false;
  databaseError = false;
  filterError = false;
  filterMessageFlag = false;
  selectedItems: any[] = [];
  noItemsFound = false;

  displayedColumns: string[] = [
    'projectName',
    'projectStatus',
    'questionare',
    'projectScope',
    'start',
    'end',
    'reportDue',
    'pentestDuration',
    'iko',
    'tko',

  ];
  dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  selection = new SelectionModel<ProjectInterface>(true, []);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;
  filters: string = '';

  length: number | undefined;

  constructor(
    private projectsCountService: GetProjectsCountService,
    private getProjectsService: GetProjectsServiceService,
    private router: Router,
    private dialog: MatDialog,
    private getRoleService: GetRoleService) { }

  userRole: string = 'admin';
  userId: string = '';

  ngOnInit(): void {

    this.getRole();
    this.getInitItems();
    this.getLength();
  }

  async getRole() {
    this.userRole = await this.getRoleService.getRole();

    
    if (this.userRole == 'Not signed in!') {
      this.userRole = 'admin';
    }
  }

  async getLength() {
    this.length = await this.projectsCountService.getNumberOfProjects();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  getStatusString(status: number): string {
    switch (status) {
      case 1:
        return 'Requested';
      case 2:
        return 'Planned';
      case 3:
        return 'In progress';
      case 4:
        return 'Finished';
      case 5:
        return 'Cancelled';
      case 6:
        return 'On hold';
      default:
        return 'TBS';
    }
  }

  getQuestionareString(questionare: number): string {
    switch (questionare) {
      case 1:
        return 'TBS';
      case 2:
        return 'Sent';
      case 3:
        return 'Received';
      default:
        return '-';
    }
  }

  getScopeString(scope: number): string {
    switch (scope) {
      case 1:
        return 'TBS';
      case 2:
        return 'Sent';
      case 3:
        return 'Confirmed';
      case 4:
        return 'Signed';
      default:
        return '-';
    }
  }

  async getInitItems() {
    this.isLoading = true;
    this.databaseError = false;
    this.filterError = false;

    try {
      const response = await this.getProjectsService.getProjects(15, 1, '', 0, true);

      if (response === "No data available.") {
        this.databaseError = false;
      } else {
        this.projects = response;
        this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
      }
    } catch (error) {
      this.databaseError = true;
    } finally {
      this.isLoading = false;
    }
  }

  getSelectedItems(): void {
    for (const item of this.selection.selected) {
      this.selectedItems.push(item);
    }
  }

  async handlePageChange() {
    this.isLoading = true;
    this.databaseError = false;

    try {
      const response = await this.getProjectsService.getProjects(this.paginator.pageSize, this.paginator.pageIndex + 1, '', 0, true);

      if (response === "No data available.") {
        this.databaseError = false;
      } else {
        this.projects = response;
        this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
      }
    } catch (error) {
      this.databaseError = true;
    } finally {
      this.isLoading = false;
    }
  }



  getStatusColor(element: any): string {
    switch (element.projectStatus) {
      case 1:
        return '#FDDDCB';
      case 2:
        return '#CAC8E0';
      case 3:
        return '#FFF3BF';
      case 4:
        return '#BFE6CD';
      case 5:
        return '#F9BFC7';
      case 6:
        return '#CEEFFB';
      default:
        return '#E9D1D4';
    }
  }

  isChecked(row: any): boolean {
    return this.selection.isSelected(row);
  }

  truncateComment(comment: string, maxLength: number): string {
    if (comment.length <= maxLength) {
      return comment;
    } else {
      return comment.substr(0, maxLength) + '...';
    }
  }

  navigateToPage(): void {
    this.router.navigate(['/project-management'])
  }
}

