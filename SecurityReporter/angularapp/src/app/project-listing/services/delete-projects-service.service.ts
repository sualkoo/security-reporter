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
    this.deleteEndPointURL = '/Project/delete';
  }

  public async deletePMProjects(data: Array<string>): Promise<any> {
    try {
      const response = await this.http
        .post(this.deleteEndPointURL, data, { observe: 'response' })
        .toPromise();
      return response;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}
