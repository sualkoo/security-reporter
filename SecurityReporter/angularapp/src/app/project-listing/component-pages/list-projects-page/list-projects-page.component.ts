import { Component, OnInit } from '@angular/core'
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { DataGridComponentComponent } from '../../components/data-grid-component/data-grid-component.component';

@Component({
  selector: 'app-list-projects-page',
  templateUrl: './list-projects-page.component.html',
  styleUrls: ['./list-projects-page.component.css'],
  imports: [MatButtonModule, DataGridComponentComponent],
  standalone: true
})

export class ListProjectsPageComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit() {
  }

  navigateToPage(): void {
    this.router.navigate(['/project-management'])
  }
}
