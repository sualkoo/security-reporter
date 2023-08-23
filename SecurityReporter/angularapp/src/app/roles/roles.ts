import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../project-search/providers/notification.service';

@Injectable()
export class Roles implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  async canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Promise<boolean> {
    const allowedRoles: string[] = route.data.allowedRoles;
    var userRole = await this.authService.getRole();

    if (userRole == 'Not signed in!') {
      userRole = '';
    }

    console.log(userRole);

    if (allowedRoles.includes(userRole)) {
      return true;
    } else {
      this.router.navigate(['welcome']);
      this.notificationService.displayMessage(
        "You're not allowed to access this page.",
        'warning'
      );
      return false;
    }
  }
}
