import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class Roles implements CanActivate {
  constructor(private router: Router, private getRoleService: AuthService) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const allowedRoles: string[] = route.data.allowedRoles;
    var userRole = await this.getRoleService.getRole();

    if (userRole == 'Not signed in!') {
      userRole = 'admin';
    }

    console.log(userRole)

    if (allowedRoles.includes(userRole)) {
      return true;
    } else {
      this.router.navigate(['welcome']);
      return false;
    }
  }
}
