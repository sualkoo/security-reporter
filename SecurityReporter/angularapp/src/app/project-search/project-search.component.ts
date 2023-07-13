import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProjectDataService } from '../project-data-service';

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search.component.html',
  styleUrls: ['./project-search.component.css'],
})
export class ProjectSearchComponent implements OnInit {
  constructor(private projectDataService: ProjectDataService) {}

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
              }
            );
          } else {
            // Incorrect zip file
            // Display eror message that says the zip is invalid
            console.error('Zip file does not meet required format');
          }
        });
    }
  }
}
