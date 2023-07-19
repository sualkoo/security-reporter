import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { GetProjectsCountService } from '../../services/get-projects-count.service';
import { GetProjectsServiceService } from '../../services/get-projects-service.service';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';

@Component({
  selector: 'app-delete-data-grid-component',
  templateUrl: './delete-data-grid-component.component.html',
  styleUrls: ['./delete-data-grid-component.component.css'],
  standalone: true,
  imports: [MatTableModule, MatCheckboxModule, MatPaginatorModule],
})
export class DeleteDataGridComponentComponent implements AfterViewInit {
  projects: ProjectInterface[] = [];
  selectedItems: any[] = [];

  displayedColumns: string[] = [
    'select',
    'id',
    'projectName',
    'projectStatus',
    'start',
    'end',
  ];

  dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  selection = new SelectionModel<ProjectInterface>(true, []);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  length: number | undefined;

  constructor(private projectsCountService: GetProjectsCountService, private getProjectsService: GetProjectsServiceService) { }

  ngOnInit(): void {
    this.getSelectedItems();
    this.getLength();
  }

  async getLength() {
    this.length = await this.projectsCountService.getNumberOfProjects();
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

  checkboxLabel(row?: ProjectInterface): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }

    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${this.projects.indexOf(row) + 2}`;
  }

  handleCheckboxChange(row: ProjectInterface): void {
    this.selection.toggle(row);

    if (this.selection.isSelected(row)) {
      this.selectedItems.push(row);
    } else {
      const index = this.selectedItems.findIndex(item => item.id === row.id);
      if (index !== -1) {
        this.selectedItems.splice(index, 1);
      }
    }
  }

  getStatusString(status: number): string {
    switch (status) {
      case 0:
        return 'TBS';
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
        return '-';
    }
  }

  getQuestionareString(questionare: number): string {
    switch (questionare) {
      case 0:
        return 'TBS';
      case 1:
        return 'Sent';
      case 2:
        return 'Received';
      default:
        return '';
    }
  }

  getScopeString(scope: number): string {
    switch (scope) {
      case 0:
        return 'TBS';
      case 1:
        return 'Sent';
      case 2:
        return 'Confirmed';
      case 3:
        return 'Signed';
      default:
        return '';
    }
  }

  async getInitItems() {
    this.projects = await this.getProjectsService.getProjects(15, 1);
    this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  }

  getSelectedItems(): void {
    for (const item of this.selection.selected) {
      this.selectedItems.push(item);
    }
  }

  async handlePageChange() {
    this.projects = await this.getProjectsService.getProjects(this.paginator.pageSize, this.paginator.pageIndex + 1);
    this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);

    this.selection.clear();
    for (const item of this.selectedItems) {
      const foundItem = this.projects.find(project => project.id === item.id);
      if (foundItem) {
        this.selection.select(foundItem);
      }
    }
  }

  getStatusColor(projectStatus: number): string {
    switch (projectStatus) {
      case 0:
        return 'white';
      case 1:
        return '#E9D1D4';
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
        return '';
    }
  }
}
