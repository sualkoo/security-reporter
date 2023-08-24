import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CheckFileExistsService {
  private checkFileExistsEndpointURL = '/Project/checkFileExists';

  constructor(private http: HttpClient) { }

  checkFileExists(projectId: string): Promise<boolean> {
    const url = `${this.checkFileExistsEndpointURL}/${projectId}`;

    return new Promise<boolean>((resolve, reject) => {
      const headers = new HttpHeaders().set('Accept', 'application/json');
      this.http.get<boolean>(url, { headers }).subscribe(
        (response) => {
          resolve(response);
        },
        (error) => {
          console.error(error);
          reject(error);
        }
      );
    });
  }
}

