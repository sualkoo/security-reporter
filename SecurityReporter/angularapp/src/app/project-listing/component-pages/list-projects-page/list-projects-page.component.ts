import { Component, OnInit } from '@angular/core'
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { DataGridComponentComponent } from '../../components/data-grid-component/data-grid-component.component';
import { DeletePopupComponentComponent } from '../../components/delete-popup-component/delete-popup-component.component';

@Component({
  selector: 'app-list-projects-page',
  templateUrl: './list-projects-page.component.html',
  styleUrls: ['./list-projects-page.component.css'],
  imports: [MatButtonModule, DataGridComponentComponent],
  standalone: true
})

export class ListProjectsPageComponent implements OnInit {
  constructor(private router: Router, private dialog: MatDialog) { }

  ngOnInit() {
  }

  navigateToPage(): void {
    this.router.navigate(['/project-management'])
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(DeletePopupComponentComponent, {
      width: '400px', 
    });

    dialogRef.afterClosed().subscribe((result) => {
      
    });
  }
}
