import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProjectInterface, projectOfferStatusIndex } from '../../project-management/interfaces/project-interface';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class GetProjectService {
  private getEndPointURL = '/Project/getProject';

  constructor(private http: HttpClient) { }

  public getProjectById(projectId: string): Promise<any> {
    const params = { id: projectId };

    return new Promise((resolve, reject) => {
      const headers = new HttpHeaders().set('Accept', 'application/json');
      this.http.get<ProjectInterface>(this.getEndPointURL, { headers, params }).subscribe(
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
