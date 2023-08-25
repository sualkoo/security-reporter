import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { GetProjectsServiceService } from '../../services/get-projects-service.service';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { DeletePopupComponentComponent } from '../../components/delete-popup-component/delete-popup-component.component';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatIconModule } from '@angular/material/icon';
import { FiltersComponent } from '../filters/filters.component';
import { ExpansionPanelComponent } from '../expansion-panel/expansion-panel.component';
import { AuthService } from '../../../services/auth.service';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { SortData } from '../../interfaces/sort-data';
 
@Component({
  selector: 'app-data-grid-component',
  templateUrl: './data-grid-component.component.html',
  styleUrls: ['./data-grid-component.component.css'],
  standalone: true,
  imports: [
    MatTableModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    CommonModule,
    MatButtonModule,
    MatTooltipModule,
    MatIconModule,
    FiltersComponent,
    ExpansionPanelComponent,
    MatSortModule,
  ],
})
export class DataGridComponentComponent implements AfterViewInit {
  @ViewChild(MatSort) sort!: MatSort;
  projects: ProjectInterface[] = [];
  checkedRows: Set<any> = new Set<any>();
  isLoading = false;
  databaseError = false;
  filterError = false;
  filterMessageFlag = false;
  selectedItems: any[] = [];
  noItemsFound = false;
  sortingDirections: boolean = false;

  displayedColumns: string[] = [
    'select',
    'projectName',
    'edit',
    'projectStatus',
    'questionare',
    'projectScope',
    'start',
    'end',
    'reportDue',
    'pentestDuration',
    'iko',
    'tko',
    'lastComment',
  ];
  dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  selection = new SelectionModel<ProjectInterface>(true, []);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;
  filters: string = '';
  count: number | undefined;
  sortData: SortData = {
    columnNumber: 0,
    sortingDescDirection: false,
  };
  constructor(private getProjectsService: GetProjectsServiceService, private router: Router, private dialog: MatDialog, private authService: AuthService) { }

  userRole: string = 'admin';

  async ngOnInit() {
    await this.getRole();
    await this.getInitItems();
  }

  async getRole() {
    this.userRole = await this.authService.getRole();

    if (this.userRole == 'Not signed in!') {
      this.userRole = 'admin';
    }
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selectedItems = [];
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
    this.getSelectedItems();
  }

  handleRowClick(row: ProjectInterface) {
    this.selection.toggle(row);
    this.handleSelectedList(row);
  }

  checkboxLabel(row?: ProjectInterface): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }

    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${
      this.projects.indexOf(row) + 2
    }`;
  }

  handleSelectedList(row: ProjectInterface) {
    if (this.selection.isSelected(row)) {
      this.selectedItems.push(row);
    } else {
      const index = this.selectedItems.findIndex((item) => item.id === row.id);
      if (index !== -1) {
        this.selectedItems.splice(index, 1);
      }
    }
  }

  handleCheckboxChange(row: ProjectInterface): void {
    this.selection.toggle(row);
    this.handleSelectedList(row);
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

  resetSorting() {
    this.sort.active = '';
    this.sort.direction = '';
    this.sort.sortChange.emit;
    this.getInitItems();
    window.location.reload(); //Reload Page
  }

  async getInitItems() {
    this.isLoading = true;
    this.databaseError = false;
    this.filterError = false;

    try {
      const response = await this.getProjectsService.getProjects(
        15,
        1,
        this.filters,
        0,
        false
      );

      if (response === 'No data available.') {
        this.databaseError = false;
      } else {
        this.projects = response.projects;
        this.count = response.count;
        this.dataSource.data = this.projects;
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
      const response = await this.getProjectsService.getProjects(
        this.paginator.pageSize,
        this.paginator.pageIndex + 1,
        this.filters,
        this.sortData.columnNumber,
        this.sortData.sortingDescDirection
      );

      if (response === 'No data available.') {
        this.databaseError = false;
      } else {
        this.projects = response.projects;
        this.count = response.count;
        this.dataSource = new MatTableDataSource<ProjectInterface>(
          this.projects
        );
      }
    } catch (error) {
      this.databaseError = true;
    } finally {
      this.isLoading = false;
    }

    this.selection.clear();
    for (const item of this.selectedItems) {
      const foundItem = this.projects.find((project) => project.id === item.id);
      if (foundItem) {
        this.selection.select(foundItem);
      }
    }
  }

  async filtersChangedHandler(filters: string) {
    console.log(filters);
    this.filters = filters;
    this.isLoading = true;

    try {
      const response = await this.getProjectsService.getProjects(
        15,
        1,
        filters,
        0,
        true
      );

      if (response === 'No data available.') {
        this.filterError = true;
        this.filterMessageFlag = true;
        this.dataSource.data = [];
      } else {
        this.projects = response.projects;
        this.count = response.count;
        this.dataSource = new MatTableDataSource<ProjectInterface>(
          this.projects
        );
      }
    } catch (error) {
      this.filterError = true;
      this.databaseError = true;
      this.dataSource.data = [];
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
    this.router.navigate(['/project-management']);
  }

  navigateToThePage(projectId: number) {
    if (projectId) {
      this.router.navigate(['/edit-project/', projectId]);
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DeletePopupComponentComponent, {
      data: this.selectedItems,
    });

    dialogRef.afterClosed().subscribe((result) => {});
  }

  areNoBoxesChecked(): boolean {
    return this.selectedItems.length === 0;
  }

  updateSelectedColumn(columnNumber: number) {
    this.sortData.columnNumber = columnNumber;
    switch (this.sortData.columnNumber) {
      case 1:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 2:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 3:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 4:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 5:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 6:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
      case 7:
        this.sortData.sortingDescDirection =
          !this.sortData.sortingDescDirection;
        this.handlePageChange();
        break;
    }
  }
}
