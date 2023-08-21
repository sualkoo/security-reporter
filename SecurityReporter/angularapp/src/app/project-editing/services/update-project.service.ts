import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ProjectInterface } from '../../project-management/interfaces/project-interface';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UpdateProjectService {
  private updateEndPointURL: string;

  constructor(private http: HttpClient) {
    this.updateEndPointURL = '/Project/update';
  }

  public updateProject(data: ProjectInterface): Observable<any> {
    return this.http.put(this.updateEndPointURL, data, { observe: 'response' }).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorResponse = error.error;

        const title = errorResponse.title;
        const status = error.status;
        const errors = errorResponse.errors;

        return throwError({ title, status, errors });
      })
    );
  }
}
