import { Component } from '@angular/core';
import { Router } from '@angular/router';
@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css', '../project-search.css'],
})
export class LandingPageComponent {
  constructor(private router: Router) { }
  navigateToPage(page: string): void {
    this.router.navigate([page]);
  }

  openLoginLink(): void {
    window.open("https://agar.io/#ffa", "_blank");
  }
}
