import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AlertService } from '../../project-management/services/alert.service';

@Injectable({
  providedIn: 'root'
})
export class DeleteService {
  private uploadURL = '/Project/deleteBlobFile';

  constructor(private http: HttpClient, private alertService: AlertService) { }

  public delete(destination: string, id: string): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.delete(this.uploadURL + "?destination=" + destination + "&id=" + id).subscribe(
        (response) => {
          resolve(response);

          this.alertService.showSnackbar(
            'File deleted successfully.',
            'Close',
            'green-alert'
          );

          window.location.reload();
        },
        (error) => {
          console.error(error);
          resolve(error);
            this.alertService.showSnackbar(
              'Error occured during deleting the file.',
              'Close',
              'red-alert'
            );
        }
      );
    });
  }
}
