import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private uploadURL = '/Project/upload';

  constructor(private http: HttpClient) { }

  public upload(file: File, destination: string, id: string): Promise<any> {
    const formData = new FormData();
    formData.append('file', file);

    return new Promise((resolve, reject) => {
      this.http.post(this.uploadURL + "?destination=" + destination + "&id=" + id, formData).subscribe(
        (response) => {
          resolve(response)
        },
        (error) => {
          console.error(error);
          reject(error);
        }
      );
    });
  }
}
