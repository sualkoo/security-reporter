import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class loginGuard implements CanActivate {

  constructor(private router: Router, private loginService: AuthService) {}

  async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
      var response = await this.loginService.sendLoginInfo("username", "password");
      if (response.status == 409) {
        console.log(response.status)
        this.router.navigate(['/welcome'])
        return false
      }

    return true
  }
}
