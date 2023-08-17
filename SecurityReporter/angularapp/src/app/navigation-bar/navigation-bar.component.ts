import {Component, Input} from '@angular/core';
@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent {
  showNavigation: boolean = false;
  toggleNavigation() {
    this.showNavigation = !this.showNavigation;
  }

  @Input() public mail: string = "samuelpopjak@outlook.sk";

  public showIntitials = false;
  public initials: string = "";
  public role: string = "admin";
  public isLoggedIn: boolean = true;

  ngOnInit() {
    this.createInitials();
  }


  private createInitials(): void {
    this.initials = this.mail.charAt(0).toUpperCase();
  }

  login() {
    this.isLoggedIn = !this.isLoggedIn;
  }
}
