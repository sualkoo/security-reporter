import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProjectInterface, projectOfferStatusIndex } from '../../project-management/interfaces/project-interface';


@Injectable({
  providedIn: 'root'
})
export class GetProjectService {
  private getEndPointURL = 'https://localhost:7075/Project/getProject';

  constructor(private http: HttpClient) { }

  public getProjectById(projectId: string): Observable<ProjectInterface> {
    const params = { id: projectId };
    return this.http.get<ProjectInterface>(this.getEndPointURL, { params }).pipe(
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
