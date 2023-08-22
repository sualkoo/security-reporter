import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AlertService } from '../project-management/services/alert.service';
import { AuthService } from '../services/auth.service';
import { LandingPageComponent } from '../landing-page/landing-page.component';

@Injectable({
  providedIn: 'root'
})
export class AutoLogoutService {
  private countdown: number = 900; // 15 minutes in seconds
  private timer: any;
  private isLoggingOut: boolean = false; 

  constructor(private snackBar: MatSnackBar, private router: Router, private alertService: AlertService, private logOut: AuthService) {
    this.initInteractionsListener();
  }

  initInteractionsListener() {
    window.addEventListener('mousemove', () => this.resetTimer());
    window.addEventListener('keydown', () => this.resetTimer());
    window.addEventListener('click', () => this.resetTimer());
    this.startTimer();
  }

  public startTimer() {
    this.timer = setInterval(() => {
      if (this.countdown > 0) {
        this.countdown--;

        if (this.countdown === 60) {
          this.showSnackbar();
        }
      } else {
        this.logout();
      }
    }, 1000);

  }

  resetTimer(): void {
    clearInterval(this.timer);
    this.countdown = 900; // Reset to 15 minutes
    this.startTimer();
  }

  private showSnackbar(): void {
    this.alertService.showSnackbar('1 minute remaining.', 'Extend', 'red-alert', 5000);
  }

  private logout(): void {
    if (!this.isLoggingOut) {
      this.isLoggingOut = true;
      if (this.logOut.logout()) {
        this.alertService.showSnackbar('You are being logged out', 'Close', 'red-alert', 5000);
      }
      else {
        this.alertService.showSnackbar('Error happened during logout', 'Close', 'red-alert', 5000);
      }
      clearInterval(this.timer);
    }
  }

  ngOnDestroy() {
    window.removeEventListener('mousemove', () => this.resetTimer());
    window.removeEventListener('keydown', () => this.resetTimer());
    window.removeEventListener('click', () => this.resetTimer());
  }
}
