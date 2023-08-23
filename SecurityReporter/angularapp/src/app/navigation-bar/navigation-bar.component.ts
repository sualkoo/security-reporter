import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css'],
})
export class NavigationBarComponent implements OnInit, OnDestroy {
  constructor(private authService: AuthService, private router: Router) {}

  showNavigation: boolean = false;
  toggleNavigation() {
    this.showNavigation = !this.showNavigation;
  }

  public photo: string = '';

  showInitials = false;
  initials: string = '';
  role = '';
  roleSub?: Subscription;
  isLoggedIn: boolean = false;
  isLoggedInSub?: Subscription;
  actualNavItem: string = '/project-search';

  setNavItem(name: string): void {
    this.actualNavItem = name;
  }

  ngOnInit() {
    // Updates navbar whenever logged in state of user changes
    this.isLoggedInSub = this.authService.getIsLoggedIn().subscribe((res) => {
      this.isLoggedIn = res;
      this.setNavbar();
    });
    // Updates navbar whenever role of user changes
    this.roleSub = this.authService.getCurrentUserRole().subscribe((res) => {
      this.role = res;
      this.setNavbar();
    });
  }

  ngOnDestroy(): void {
    this.isLoggedInSub?.unsubscribe();
    this.roleSub?.unsubscribe();
  }

  setNavbar() {
    if (!this.photo) {
      this.showInitials = true;
      this.createInitials();
    }
    if (!this.isLoggedIn) {
      this.menuItems = this.defaultMenuItems;
    } else {
      switch (this.role) {
        case 'admin':
          this.menuItems = this.adminMenuItems;
          break;
        case 'pentester':
          this.menuItems = this.pentesterMenuItems;
          break;
        case 'client':
          this.menuItems = this.clientMenuItems;
          break;
        case 'coordinator':
          this.menuItems = this.coordinatorMenuItems;
          break;
        default:
          this.menuItems = this.defaultMenuItems;
      }
    }
  }

  private createInitials(): void {
    this.initials = this.role.charAt(0).toUpperCase();
  }

  onLogin() {
    this.router.navigate(['log-in']);
  }

  onLogout() {
    this.authService.logoutAsync().then(() => {
      this.router.navigateByUrl('/log-in');
    });
  }

  menuItems: { text: string; link: string; disabled?: boolean }[] = [];

  adminMenuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'Project Search', link: '/project-search', disabled: false },
    {
      text: 'Project Management',
      link: '/project-management',
      disabled: false,
    },
    { text: 'Dashboard', link: '/dashboard', disabled: false },
  ];

  defaultMenuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];

  pentesterMenuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'Project Search', link: '/project-search', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
  ];

  coordinatorMenuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    {
      text: 'Project Management',
      link: '/project-management',
      disabled: false,
    },
    { text: 'Dashboard', link: '/dashboard', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
  ];

  clientMenuItems: { text: string; link: string; disabled?: boolean }[] = [
    { text: 'Home', link: '/welcome', disabled: false },
    { text: 'About pentest', link: '/about-pentests', disabled: false },
    { text: 'Order a pentest', link: '/in-development', disabled: true },
  ];
}
