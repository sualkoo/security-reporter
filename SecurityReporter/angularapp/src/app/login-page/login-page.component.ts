import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../services/auth.service';
import { error } from 'cypress/types/jquery';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css'],
})
export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private loginService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onLoginSubmit() {
    if (this.loginForm.valid) {
      const username = this.loginForm.value.username;
      const password = this.loginForm.value.password;

      //DUMMY TEMPORARY LOGIN
      this.loginService.sendLoginInfo(username, password).then((data) => {
        if (data.status == 200) {
          console.log('Login successful');

          this.loginService.getRole().then((role) => {
            if (role === 'default') {
              // window.location.href = 'default-page';
              this.router.navigateByUrl('/default-page');
            } else {
              this.router.navigateByUrl('/welcome');
              // window.location.href = 'welcome';
            }
          });
        } else if (data.status == 400) {
          this.snackBar.open('Bad credentials.', 'Close', {
            duration: 5000,
            panelClass: 'red-alert',
          });
        }
      });
    }
  }
}
