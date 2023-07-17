import { Component } from '@angular/core';
import { MatPaginatorModule } from '@angular/material/paginator';
import { GetProjectsCountService } from '../../services/get-projects-count.service';

@Component({
  selector: 'app-paginator-component',
  templateUrl: './paginator-component.component.html',
  styleUrls: ['./paginator-component.component.css'],
  standalone: true,
  imports: [MatPaginatorModule],
})
export class PaginatorComponentComponent {
  length: number | undefined;

  constructor(private projectsCountService: GetProjectsCountService) { }

  ngOnInit(): void {
    this.getLength();
  }

  async getLength() {
    this.length = await this.projectsCountService.getNumberOfProjects();
    console.log(this.length);
  }
}
