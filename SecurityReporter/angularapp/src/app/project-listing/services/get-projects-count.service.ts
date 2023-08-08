import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GetProjectsCountService {
  private getCountEndPointURL: string;

  constructor(private http: HttpClient) {
    this.getCountEndPointURL = '/Project/count';
  }

  public getNumberOfProjects(): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.get(this.getCountEndPointURL).subscribe(
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
