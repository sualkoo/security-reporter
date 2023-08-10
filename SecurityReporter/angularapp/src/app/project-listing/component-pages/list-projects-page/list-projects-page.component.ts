import { Component, OnInit } from '@angular/core';
import { DataGridComponentComponent } from '../../components/data-grid-component/data-grid-component.component';
import { AutoLogoutService } from '../../../services/auto-logout.service';

@Component({
  selector: 'app-list-projects-page',
  templateUrl: './list-projects-page.component.html',
  styleUrls: ['./list-projects-page.component.css'],
  imports: [DataGridComponentComponent],
  standalone: true
})
export class ListProjectsPageComponent implements OnInit { // Implement OnInit interface
  constructor(private autoLogoutService: AutoLogoutService) { }

  ngOnInit(): void {
    this.autoLogoutService.startTimer();
  }
}
