import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { GetRoleService } from '../../../shared/services/get-role.service';
import { FiltersComponent } from '../../../project-listing/components/filters/filters.component';
import { ExpansionPanelComponent } from '../../../project-listing/components/expansion-panel/expansion-panel.component';
import { GetBacklogService } from './Services/get-backlog.service';

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
    private getBacklogService: GetBacklogService,
    private getRoleService: GetRoleService) { }

  userRole: string = 'admin';
  userId: string = '';

  ngOnInit(): void {

    this.getRole();
    this.getInitItems();
  }

  async getRole() {
    this.userRole = await this.getRoleService.getRole();

    
    if (this.userRole == 'Not signed in!') {
      this.userRole = 'admin';
    }
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

  async getInitItems() {
    this.isLoading = true;
    this.databaseError = false;
    this.filterError = false;

    try {
      const pageSize = 15;
      const pageNumber = 1;

      const response = await this.getBacklogService.getBacklogData(pageSize, pageNumber).toPromise();

      if (response.length === 0) {
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
}

