import { Component } from '@angular/core';

@Component({
  selector: 'app-searchbar',
  templateUrl: './searchbar.component.html',
  styleUrls: ['./searchbar.component.css']
})
export class SearchbarComponent {
  searchQuery: string | undefined;
  filters: string[] = ['Project name', 'Details', 'Refferences', 'Impact', 'CWE', 'Repeatability'];
  selectedFilters: { [key: string]: boolean } = {};
  isLoading: boolean = false;

  search(): void {
    this.isLoading = true;
    setTimeout(() => {
      // After the operation completes, reset isLoading to false
      this.isLoading = false;
    }, 2000);
    console.log('Performing search for:', this.searchQuery);
    this.searchQuery = ''; 
  }
}
