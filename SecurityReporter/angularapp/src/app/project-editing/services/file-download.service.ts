import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileDownloadService {
  private getDownloadEndpointURL = '/Project/download';

  constructor(private http: HttpClient) { }

  getDownloadFile(fileName: string): Promise<any> {
    const params = new HttpParams().set('name', fileName);

    return new Promise((resolve, reject) => {
      const headers = new HttpHeaders().set('Accept', 'application/json');
      this.http.get(this.getDownloadEndpointURL, { headers, params, responseType: 'blob' }).subscribe(
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
