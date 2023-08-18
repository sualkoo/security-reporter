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

  public showInitials = true;
  public initials: string = "";
  public role: string = "admin";
  public isLoggedIn: boolean = false;
  public actualNavItem: string = "/project-search";
  public circleColor: string = "slate-grey";

  setNavItem(name: string): void {
    this.actualNavItem = name;
  }

  ngOnInit() {
    this.createInitials();
    this.setNavbar();
  }
    setNavbar() {
      if (!this.isLoggedIn) {
        this.menuItems = this.defaultMenuItems;
      } else {
        switch (this.role) {
          case "admin":
            this.menuItems = this.adminMenuItems;
            break;
          case "pentester":
            this.menuItems = this.pentesterMenuItems;
            break;
          case "client":
            this.menuItems = this.clientMenuItems;
            break;
          case "coordinator":
            this.menuItems = this.coordinatorMenuItems;
            break;
          default:
            this.menuItems = this.defaultMenuItems;
        }
      }
    }


  private createInitials(): void {
    this.initials = this.mail.charAt(0).toUpperCase();
  }

  login() {
    this.isLoggedIn = true;
    this.setNavbar();
  }

  logout() {
    this.isLoggedIn = false;
    this.setNavbar();
  }

  menuItems: { text: string, link: string, disabled?: boolean }[] = [];

  adminMenuItems: { text: string, link: string, disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'Project Search', link: '/project-search', disabled: false },
    { text: 'Project Management', link: '/project-management', disabled: false },
    { text: 'Dashboard', link: '/dashboard', disabled: false },
  ];

  defaultMenuItems: { text: string, link: string, disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];

  pentesterMenuItems: { text: string, link: string, disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];

  coordinatorMenuItems: { text: string, link: string, disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];

  clientMenuItems: { text: string, link: string, disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];
}
