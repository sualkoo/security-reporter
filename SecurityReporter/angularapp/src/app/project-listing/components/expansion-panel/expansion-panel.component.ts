import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { FiltersComponent } from '../filters/filters.component';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.css'],
  standalone: true,
  imports: [MatExpansionModule, FiltersComponent, MatIconModule],
})
export class ExpansionPanelComponent {
  panelOpenState = false;
}
