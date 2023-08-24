import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../project-management/services/alert.service';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private uploadURL = '/Project/upload';

  constructor(private http: HttpClient, private alertService: AlertService) { }

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
          resolve(error);

          if (error.status == 201) {
            this.alertService.showSnackbar(
              'File uploaded successfully.',
              'Close',
              'green-alert'
            );
          } else {
            this.alertService.showSnackbar(
              'Error occured during uploading the file.',
              'Close',
              'red-alert'
            );
          }
        }
      );
    });
  }
}
