import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable()
export class Roles implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const allowedRoles: string[] = route.data.allowedRoles;
    const userRole = 'admin';

    if (allowedRoles.includes(userRole)) {
      return true;
    } else {
      this.router.navigate(['']);
      return false;
    }
  }
}
