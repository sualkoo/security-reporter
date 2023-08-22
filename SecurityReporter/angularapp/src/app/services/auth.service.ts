import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private logOutUrl = "/logout";
  private loginUrl = "/login";
  private getRoleEndpointURL = '/role';
  status_code = 0;

  constructor(private http: HttpClient) { }

  public logout(): boolean {
    this.http
      .get(this.logOutUrl, { withCredentials: true })
      .subscribe(
        response => {
          console.log('Request successful. Status code: 200');
          return true;
        },
        error => {
          console.error('An error occurred:', error);
          return false;
        }
      );
    return false;
  }

  public sendLoginInfo(username: string, password: string): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
      .post(this.loginUrl + `?name=${username}&password=${password}`, null, { withCredentials: true })
      .subscribe(response => resolve(response), error => {
        resolve(error)
      })
    })
  }

  public getRole(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.get(this.getRoleEndpointURL, { responseType: 'text' as 'json' }).subscribe(
        (response) => {
          resolve(response);
        },
        (error) => {
          resolve(error.error);
        }
      );
    });
  }
}
