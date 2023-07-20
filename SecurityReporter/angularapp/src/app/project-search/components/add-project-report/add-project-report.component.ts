import { Component } from '@angular/core';
import { NotificationService } from '../../providers/notification.service';
import { ProjectDataService } from '../../providers/project-data-service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-project-report',
  templateUrl: './add-project-report.component.html',
  styleUrls: ['./add-project-report.component.css', '../../project-search.css'],
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
    if (!this.uploadedFile) {
      this.notificationService.displayMessage("Please select a file", "info");
      throw new Error("File not selected.");
    }

    const formData = new FormData();
    formData.append('file', this.uploadedFile!);

    this.projectDataService
      .validateZipFile(new File([this.uploadedFile], this.uploadFile.name))
      .then((isValid) => {
        if (isValid) {
          // Correct zip file
          this.projectDataService.postZipFile(formData).subscribe(
            (response) => {
              console.log(response);
              this.notificationService.displayMessage("Report successfully saved to DB.", "success");
            },
            (errorResponse: HttpErrorResponse) => {
              this.notificationService.displayMessage(errorResponse.error.details, 'error');
      })
        } else {
          // Incorrect zip file
          // Display eror message that says the zip is invalid
          this.notificationService.displayMessage('Zip file is incorrect', 'warning');
        }
      })
      .catch((err) => {
        console.error(err.message);
        this.notificationService.displayMessage('Zip file is incorrect', 'warning');
      });
  }
}
