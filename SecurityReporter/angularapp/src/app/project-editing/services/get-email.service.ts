import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class GetEmailService {
  private getRoleEndpointURL: string;

  constructor(private http: HttpClient) {
    this.getRoleEndpointURL = '/email';
  }

  public getEmail(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http
        .get(this.getRoleEndpointURL, { responseType: 'text' as 'json' })
        .subscribe(
          (response) => {
            resolve(response);
          },
          (error) => {
            reject(error);
          }
        );
    });
  }
}
