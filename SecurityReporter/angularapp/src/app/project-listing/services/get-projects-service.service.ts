import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProjectInterface } from '../../project-management/interfaces/project-interface';

@Injectable({
  providedIn: 'root'
})
export class GetProjectsServiceService {

  private getProjectEndPointURL: string;

  constructor(private http: HttpClient) {
    this.getProjectEndPointURL = 'https://localhost:7075/Project/retrieve';
  }

  public getProjects(pageSize: number, pageNumber: number): Promise<any> {
    this.getProjectEndPointURL = this.getProjectEndPointURL + '?pageSize=' + pageSize + '&pageNumber=' + pageNumber;
    return new Promise((resolve, reject) => {
      this.http.get(this.getProjectEndPointURL).subscribe(
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
