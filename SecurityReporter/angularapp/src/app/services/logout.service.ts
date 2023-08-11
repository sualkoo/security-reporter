import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {
  private logOutUrl = "/logout";
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
}
