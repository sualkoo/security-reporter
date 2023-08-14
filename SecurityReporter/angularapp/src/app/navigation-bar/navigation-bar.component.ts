import { Component } from '@angular/core';
@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent {
  actualNavItem: string = "";
  showNavigation: boolean = false;
  setNavItem(navItem: string): void {
    this.actualNavItem = navItem;
  }
  toggleNavigation() {
    this.showNavigation = !this.showNavigation;
  }
}
