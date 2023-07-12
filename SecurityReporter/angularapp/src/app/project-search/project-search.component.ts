import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProjectDataServiceService} from "../project-data-service.service";

@Component({
  selector: 'app-project-search',
  templateUrl: './project-search.component.html',
  styleUrls: ['./project-search.component.css']
})
export class ProjectSearchComponent

  implements OnInit{

  uploadedFile?: Blob;
  constructor(private searchService: ProjectDataServiceService,
              private http: HttpClient,


  ) {}
  ngOnInit(): void {
  }



  upload({event}: { event: any}) {
    this.uploadedFile = event.target.files[0];
  }

  uploadFile() {

    if (this.uploadedFile != undefined) {
      const formData = new FormData();

      formData.append('file', this.uploadedFile!);

      console.log(this.uploadedFile);
      console.log(formData);

      this.searchService.postZipFile(formData).subscribe(
        (response) => {
          console.log(response);
        },
        (error) => {
          console.log(error);
        }
      );
    }
  }
}
