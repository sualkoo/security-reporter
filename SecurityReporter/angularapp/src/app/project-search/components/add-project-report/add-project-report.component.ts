import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NotificationService } from '../../providers/notification.service';
import { ProjectReportService } from '../../providers/project-report-service';
import { HttpErrorResponse } from '@angular/common/http';
import { ProjectReport } from '../../interfaces/project-report.model';
import { ErrorResponse } from '../../interfaces/error-response';


@Component({
  selector: 'app-add-project-report',
  templateUrl: './add-project-report.component.html',
  styleUrls: ['./add-project-report.component.css', '../../project-search.css']
})
export class AddProjectReportComponent implements OnInit{
  constructor(
    private projectDataService: ProjectReportService,
    private notificationService: NotificationService) {
  }
  uploadedFile?: Blob;

  ngOnInit(): void {
  }

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
              console.error(errorResponse);
              // If error has details, show first detail, otherwise show common message
              this.notificationService.displayMessage(
                errorResponse.error.details ? (errorResponse.error as ErrorResponse).details[0] : (errorResponse.error as ErrorResponse).message,
                'error')
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
