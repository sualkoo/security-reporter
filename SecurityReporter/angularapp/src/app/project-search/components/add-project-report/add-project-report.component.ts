import { Component, ElementRef, EventEmitter, HostListener, OnInit, Output, ViewChild } from '@angular/core';
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
  @ViewChild('fileInputRef', { static: false }) fileInputRef!: ElementRef<HTMLInputElement>;
  constructor(
    private projectDataService: ProjectReportService,
    private notificationService: NotificationService,
  ) {
  }
  uploadedFile?: Blob;
  isDragOver = false;


  ngOnInit(): void {
  }

  upload({ event }: { event: any }) {
    this.uploadedFile = event.target.files[0];
  }

  deleteFile() {
    this.uploadedFile = undefined;
    this.fileInputRef.nativeElement.value = '';
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

  @HostListener('dragenter', ['$event'])
  onDragEnter(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  @HostListener('dragover', ['$event'])
  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = true;
  }

  @HostListener('dragleave', ['$event'])
  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
  }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragOver = false;
    // Handle the dropped file here
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.uploadedFile = files[0];
    }

  }
}
