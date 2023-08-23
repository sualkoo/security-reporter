import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private logOutUrl = '/logout';
  private loginUrl = '/login';
  private getRoleEndpointURL = '/role';
  private isLoggedIn = new BehaviorSubject<boolean>(false);
  private currentRole = new BehaviorSubject<string>('');
  status_code = 0;

  constructor(private http: HttpClient) {
    this.getRole();
  }

  getIsLoggedIn() {
    return this.isLoggedIn.asObservable();
  }

  getCurrentUserRole() {
    return this.currentRole.asObservable();
  }

  public logout(): boolean {
    this.http.get(this.logOutUrl, { withCredentials: true }).subscribe(
      (response) => {
        console.log('Request successful. Status code: 200');
        this.isLoggedIn.next(false);
        return true;
      },
      (error) => {
        console.error('An error occurred:', error);
        return false;
      }
    );
    return false;
  }

  public logoutAsync(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http.get(this.logOutUrl, { withCredentials: true }).subscribe(
        (response) => {
          console.log('Request successful. Status code: 200');
          this.isLoggedIn.next(false);
          resolve(true);
        },
        (error) => {
          console.error('An error occurred:', error);
          this.isLoggedIn.next(false);
          resolve(false);
        }
      );
    });
  }

  public sendLoginInfo(username: string, password: string): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
        .post(this.loginUrl + `?name=${username}&password=${password}`, null, {
          withCredentials: true,
        })
        .subscribe(
          (response) => {
            this.isLoggedIn.next(true);
            resolve(response);
          },
          (error) => {
            resolve(error);
          }
        );
    });
  }

  public getRole(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
        .get(this.getRoleEndpointURL, { responseType: 'text' as 'json' })
        .subscribe(
          (response) => {
            this.isLoggedIn.next(true);
            console.log("Current role: " + response);
            this.currentRole.next(response.toString());
            resolve(response);
          },
          (error) => {
            resolve(error.error);
          }
        );
    });
  }
}
