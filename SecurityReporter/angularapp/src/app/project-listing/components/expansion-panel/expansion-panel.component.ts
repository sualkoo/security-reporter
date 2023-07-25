import { Component, EventEmitter, Output } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { FiltersComponent } from '../filters/filters.component';
import { MatIconModule } from '@angular/material/icon';
import { DataGridComponentComponent } from '../data-grid-component/data-grid-component.component';


@Component({
  selector: 'app-expansion-panel',
  templateUrl: './expansion-panel.component.html',
  styleUrls: ['./expansion-panel.component.css'],
  standalone: true,
  imports: [MatExpansionModule, FiltersComponent, MatIconModule, DataGridComponentComponent],
})
export class ExpansionPanelComponent {

  @Output() filtersChangedEvent = new EventEmitter<string>();

  filtersChangedHandler(filters: string) {
    
    this.filtersChangedEvent.emit(filters);
  }
  panelOpenState = false;
}
