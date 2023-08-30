import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ProjectInterface } from '../../../../project-management/interfaces/project-interface';

@Injectable({
  providedIn: 'root',
})
export class GetBacklogService {
  private getProjectEndPointURL: string;

  constructor(private http: HttpClient) {
    this.getProjectEndPointURL = '/profile';
  }

  getBacklogData(pageSize: number, pageNumber: number): Promise<any> {
    this.getProjectEndPointURL =
      '/profile' + '?pageSize=' + pageSize + '&pageNumber=' + pageNumber;

    return new Promise((resolve, reject) => {
      this.http.get(this.getProjectEndPointURL).subscribe(
        (response) => {
          if (response !== null && response !== undefined) {
            resolve(response);
          } else {
            const noDataResponse = 'No data available.';
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
