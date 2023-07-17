import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-list-projects-page',
  templateUrl: './list-projects-page.component.html',
  styleUrls: ['./list-projects-page.component.css'],
  standalone:true,
  imports: [MatButtonModule]
})
export class ListProjectsPageComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit() {
  }

  navigateToPage(): void {
    this.router.navigate(['/project-management'])
  }
}
