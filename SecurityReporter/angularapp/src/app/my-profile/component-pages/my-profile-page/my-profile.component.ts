import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatTabsModule } from '@angular/material/tabs';
import { BacklogComponentComponent } from '../../components/backlog-component/backlog-component.component';



@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css'],
  standalone: true,
  imports: [MatButtonModule, BacklogComponentComponent, MatTabsModule]
})

export class MyProfileComponent {

}
