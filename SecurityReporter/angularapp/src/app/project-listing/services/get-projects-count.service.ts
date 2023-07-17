import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetProjectsCountService {

  private getCountEndPointURL: string;

  constructor(private http: HttpClient) {
    this.getCountEndPointURL = 'https://localhost:7075/Project/count';
  }

  public getNumberOfProjects(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.get(this.getCountEndPointURL).subscribe(
        (response) => {
          console.log(response);
          resolve(response); // Resolve the Promise with the response
        },
        (error) => {
          console.error(error);
          reject(error); // Reject the Promise with the error
        }
      );
    });
  }
}
