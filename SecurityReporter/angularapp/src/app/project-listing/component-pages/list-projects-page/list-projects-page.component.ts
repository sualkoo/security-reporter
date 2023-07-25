import { Component, OnInit } from '@angular/core'
import { DataGridComponentComponent } from '../../components/data-grid-component/data-grid-component.component';


@Component({
  selector: 'app-list-projects-page',
  templateUrl: './list-projects-page.component.html',
  styleUrls: ['./list-projects-page.component.css'],
  imports: [DataGridComponentComponent],
  standalone: true
})

export class ListProjectsPageComponent implements OnInit {
  ngOnInit() {
  }
}
