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
  status_code = 0;


  public sendLoginInfo(username: string, password: string): Promise<any> {
    return new Promise ((resolve, reject) => { this.http
      .post(this.loginUrl + `?name=${username}&password=${password}`, null, {  withCredentials: true  })
      .subscribe(response => resolve(response), error => {
        resolve(error)
      })
    })
  }
}
