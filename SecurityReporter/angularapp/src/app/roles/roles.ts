import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { GetRoleService } from '../shared/services/get-role.service';

@Injectable()
export class Roles implements CanActivate {
  constructor(private router: Router, private getRoleService: GetRoleService) { }

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
      this.router.navigate(['']);
      return false;
    }
  }
}
