import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeleteProjectServiceService {

  private removeProjectEndPointURL: string;

  constructor(private http: HttpClient) {
    this.removeProjectEndPointURL = 'https://localhost:7075/Project/delete';
  }
  
  public removeProjects(idList: string[]): Promise<any> {
    const promises: Promise<any>[] = [];
    for (const id of idList) {
      const urlWithId = this.removeProjectEndPointURL + '?id=' + id;
      const promise = this.http.delete(urlWithId).toPromise();
      promises.push(promise);
    }

    return Promise.all(promises);
  }
}
