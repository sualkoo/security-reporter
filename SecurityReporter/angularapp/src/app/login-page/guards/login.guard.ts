import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { LoginService } from '../services/login.service';

@Injectable({
  providedIn: 'root'
})
export class loginGuard implements CanActivate {

  constructor(private router: Router, private loginService: LoginService) {}

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