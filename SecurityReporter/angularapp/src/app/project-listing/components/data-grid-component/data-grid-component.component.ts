import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { GetProjectsCountService } from '../../services/get-projects-count.service';
import { GetProjectsServiceService } from '../../services/get-projects-service.service';
import { ProjectInterface } from '../../../project-management/interfaces/project-interface';

@Component({
  selector: 'app-data-grid-component',
  templateUrl: './data-grid-component.component.html',
  styleUrls: ['./data-grid-component.component.css'],
  standalone: true,
  imports: [MatTableModule, MatCheckboxModule, MatPaginatorModule],
})
export class DataGridComponentComponent implements AfterViewInit {
  projects: ProjectInterface[] = [];

  displayedColumns: string[] = [
    'select',
    'id',
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
    'lastComment',
  ];
  dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  selection = new SelectionModel<ProjectInterface>(true, []);

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  length: number | undefined;

  constructor(private projectsCountService: GetProjectsCountService, private getProjectsService: GetProjectsServiceService) { }

  ngOnInit(): void {
    this.getInitItems();
    this.getLength();
  }

  async getLength() {
    this.length = await this.projectsCountService.getNumberOfProjects();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;

  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: ProjectInterface): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }

    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${this.projects.indexOf(row) + 2}`;
  }

  async getInitItems() {
    this.projects = await this.getProjectsService.getProjects(15, 1);
    this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  }

  async handlePageChange() {
    this.projects = await this.getProjectsService.getProjects(this.paginator.pageSize, this.paginator.pageIndex + 1);
    this.dataSource = new MatTableDataSource<ProjectInterface>(this.projects);
  }
}
