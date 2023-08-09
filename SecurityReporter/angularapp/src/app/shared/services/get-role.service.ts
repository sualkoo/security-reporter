import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GetRoleService {

  private getRoleEndpointURL: string;

  constructor(private http: HttpClient) {
    this.getRoleEndpointURL = 'https://localhost:7075/role';
  }

  public getRole(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.get(this.getRoleEndpointURL, { responseType: 'text' as 'json' }).subscribe(
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
