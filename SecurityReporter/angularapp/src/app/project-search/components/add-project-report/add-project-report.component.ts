import { Component } from '@angular/core';
import { NotificationService } from '../../providers/notification.service';
import { ProjectDataService } from '../../providers/project-data-service';

@Component({
  selector: 'app-add-project-report',
  templateUrl: './add-project-report.component.html',
  styleUrls: ['./add-project-report.component.css'],
})
export class AddProjectReportComponent {
  constructor(
    private projectDataService: ProjectDataService,
    private notificationService: NotificationService
  ) {}

  uploadedFile?: Blob;

  ngOnInit(): void {}

  upload({ event }: { event: any }) {
    this.uploadedFile = event.target.files[0];
  }

  uploadFile() {
    if (this.uploadedFile != undefined) {
      const formData = new FormData();

      formData.append('file', this.uploadedFile!);

      this.projectDataService
        .validateZipFile(new File([this.uploadedFile], this.uploadFile.name))
        .then((val) => {
          if (val === true) {
            // Correct zip file
            this.projectDataService.postZipFile(formData).subscribe(
              (response) => {
                console.log(response);
              },
              (error) => {
                console.log(error);
                this.notificationService.displayMessage(
                  'Something went wrong on server side'
                );
              }
            );
          } else {
            // Incorrect zip file
            // Display eror message that says the zip is invalid
            this.notificationService.displayMessage('Zip file is incorrect');
          }
        });
    }
  }
}
