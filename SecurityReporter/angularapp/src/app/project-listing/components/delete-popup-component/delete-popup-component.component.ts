import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { NgIf } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Router } from '@angular/router';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { ProjectInterface } from 'src/app/project-management/interfaces/project-interface';
import { DeleteProjectsServiceService } from '../../services/delete-projects-service.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-delete-popup-component',
  templateUrl: './delete-popup-component.component.html',
  styleUrls: ['./delete-popup-component.component.css'],
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    NgIf,
    MatDialogModule,
    MatTableModule,
  ],
})

export class DeletePopupComponentComponent {
  displayedColumns: string[] = ['projectName', 'projectStatus', 'startDate', 'endDate'];
  dataSource = new MatTableDataSource<ProjectInterface>();

  constructor(
    public dialog: MatDialogRef<DeletePopupComponentComponent>,
    private router: Router,
    private service: DeleteProjectsServiceService,
    private location: Location,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {
    this.dataSource.data = data;
  }

  ngOnInit() {
    console.log(this.dataSource.data);
  }

  async DeleteItems() {
    const idList = this.dataSource.data.map(item => item.id);
    await this.service.deletePMProjects(idList).subscribe(
      (response) => {
        console.log('Items deleted successfully.');
      },
      (error) => {
        console.error('Error deleting items:', error);
      }
    );

    window.location.reload();
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

  onBackClick(): void {
    this.dialog.close();
  }
}

