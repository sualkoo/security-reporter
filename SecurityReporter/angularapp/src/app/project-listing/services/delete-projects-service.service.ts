import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class DeleteProjectsServiceService {
  private deleteEndPointURL: string;

  constructor(private http: HttpClient) {
    this.deleteEndPointURL = 'https://localhost:7075/Project/delete';
  }

  public deletePMProjects(data: Array<string>): Observable<any> {
    return this.http
      .post(this.deleteEndPointURL, data, { observe: 'response' })
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorResponse = error.error;
          const title = errorResponse.title;
          const status = error.status;
          const errors = errorResponse.errors;

          return throwError({ title, status, errors });
        })
      );
  }

  public deletePMProject(id: string): Observable<any> {
    return this.http
      .delete(this.deleteEndPointURL + '?id=' + id, { observe: 'response' })
      .pipe(
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
