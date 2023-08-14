import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginService } from './services/login.service';
import { error } from 'cypress/types/jquery';
import { GetRoleService } from '../shared/services/get-role.service';



@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  loginForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private snackBar: MatSnackBar, private loginService: LoginService, private roleService: GetRoleService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLoginSubmit() {
    if (this.loginForm.valid) {
      const username = this.loginForm.value.username;
      const password = this.loginForm.value.password;

      //DUMMY TEMPORARY LOGIN
      this.loginService.sendLoginInfo(username, password).then(data => {
        if ( data.status == 200) {
          console.log("Login successful")

          this.roleService.getRole().then(role => {
            switch (role) {
              case 'default':
                window.location.href = 'default-page';
                break;
              case 'pentester':
                window.location.href = 'project-search';
                break;
              case 'client':
              case 'coordinator':
                window.location.href = 'list-projects';
                break;
              case 'admin':
                // TODO Change redirection to /dashboard when ZA changes are brought to main
                window.location.href = 'landing-page';
                break;
            }
          });
        } 

        else if(data.status == 400){
          this.snackBar.open('Bad credentials.', 'Close', {
            duration: 5000,
            panelClass: 'red-alert'
          });
        }
      })
    }
  }
}
