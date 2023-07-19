import { Component } from '@angular/core';
import { MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { DataGridComponentComponent } from '../data-grid-component/data-grid-component.component';
import { DeleteDataGridComponentComponent } from '../delete-data-grid-component/delete-data-grid-component.component';

@Component({
  selector: 'app-delete-popup-component',
  templateUrl: './delete-popup-component.component.html',
  styleUrls: ['./delete-popup-component.component.css'],
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, DataGridComponentComponent, DeleteDataGridComponentComponent]
})
export class DeletePopupComponentComponent {
  constructor(public dialog: MatDialogRef<DeletePopupComponentComponent>, private router: Router) { }

  ngOnInit() {
    //dataGridComponent: DataGridComponentComponent
  }

  CloseDialogWindow(): void {
    this.dialog.close(); 
    //this.router.navigate(['/list-projects']); 
  }
}

