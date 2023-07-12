import { Component } from '@angular/core';

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search.component.html',
  styleUrls: ['./project-search.component.css']
})
export class ProjectSearchComponent {

  uploadedFile?: Blob;

  upload({ event }: { event: any }) {
    this.uploadedFile = event.target.files[0];
  }

  uploadFile() {
    if (this.uploadedFile) {
      console.log('Uploading file:', this.uploadedFile);
    } else {
      console.log('No file selected');
    }
  }

}
