import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProjectInterface } from '../interfaces/project-interface';

@Injectable({
  providedIn: 'root',
})
export class AddProjectService {
  private addEndPointURL: string;

  constructor(private http: HttpClient) {
    this.addEndPointURL = 'https://localhost:7075/Project/add';
  }

  public submitPMProject(data: ProjectInterface) {
    console.log(this.http.post(this.addEndPointURL, data));
    return this.http.post(this.addEndPointURL, data);
  }
}
