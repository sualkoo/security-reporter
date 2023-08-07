import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProjectInterface } from '../../project-management/interfaces/project-interface';
import { ProjectData } from '../interfaces/project-data';

@Injectable({
  providedIn: 'root'
})
export class GetProjectsServiceService {

  private getProjectEndPointURL: string;

  constructor(private http: HttpClient) {
    this.getProjectEndPointURL = '/Project/retrieve';
  }

  public getProjects(pageSize: number, pageNumber: number, filters: string): Promise<any> {
    this.getProjectEndPointURL = '/Project/retrieve' + '?pageSize=' + pageSize + '&pageNumber=' + pageNumber + filters;
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
