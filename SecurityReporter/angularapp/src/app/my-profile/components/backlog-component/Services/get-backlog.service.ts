import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ProjectInterface } from '../../../../project-management/interfaces/project-interface';

@Injectable({
  providedIn: 'root'
})
export class GetBacklogService {

  private endPointUrl = '/profile';

  constructor(private http: HttpClient) { }

  getBacklogData(pageSize: number, pageNumber: number): Observable<ProjectInterface[]> {
    const params = {
      pageSize: pageSize.toString(),
      pageNumber: pageNumber.toString()
    };


    return this.http.get<ProjectInterface[]>(this.endPointUrl, { params });
  }
}
