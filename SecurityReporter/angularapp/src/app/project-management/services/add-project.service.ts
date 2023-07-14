import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { ProjectInterface } from '../interfaces/project-interface';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AddProjectService {
  private addEndPointURL: string;

  constructor(private http: HttpClient) {
    this.addEndPointURL = 'https://localhost:7075/Project/add';
  }

  public submitPMProject(data: ProjectInterface): Observable<any> {
    return this.http.post(this.addEndPointURL, data, { observe: 'response' }).pipe(
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
