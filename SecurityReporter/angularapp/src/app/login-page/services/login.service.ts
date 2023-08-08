import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private loginUrl = "/login";

  constructor(private http: HttpClient) {  }


  public sendLoginInfo(username: string, password: string): boolean {
    console.log("Poslal login", username, password)

    this.http
      .post(this.loginUrl, {username: username, password: password}, { observe: 'response' })
      .subscribe(
        response => {
            console.log('Request successful. Status code: 200');
            // Additional handling for a successful response here
            return true;
        },
        error => {
          console.error('An error occurred:', error);
          // Additional error handling here
          return false;

        }
      );

      return false;
  }
}
