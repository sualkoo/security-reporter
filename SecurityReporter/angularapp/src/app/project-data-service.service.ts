import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ProjectDataServiceService {

  private postZipFileUrl:  string;

  constructor(private http: HttpClient) {
    this.postZipFileUrl = 'https://localhost:7075/upload'

  }

  public postZipFile(file: any){
    return this.http.post(this.postZipFileUrl, file)
  }
}
