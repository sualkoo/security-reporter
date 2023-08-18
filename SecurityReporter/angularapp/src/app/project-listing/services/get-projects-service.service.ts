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

  public getProjects(pageSize: number, pageNumber: number, filters: string, columnNumber: number, sorting: boolean): Promise<any> {
    this.getProjectEndPointURL = '/Project/retrieve' + '?pageSize=' + pageSize + '&pageNumber=' + pageNumber + filters + '&SortingColumns=' + columnNumber + '&SortingDescDirection=' + sorting;


    return new Promise((resolve, reject) => {
      this.http.get(this.getProjectEndPointURL).subscribe(
        (response) => {
          if (response !== null && response !== undefined) {
            resolve(response);
          } else {
            const noDataResponse = "No data available.";
            resolve(noDataResponse);
          }
        },
        (error) => {
          console.error(error);
          reject(error);
        }
      );
    });
  }

}
