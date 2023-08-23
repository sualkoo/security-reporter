import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import {ApiPaths} from "../../api-paths.enum";

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl: string = ApiPaths.Dashboard;
  constructor(private http: HttpClient) {
}

  public getCriticality() {
    return this.http.get<number[]>(`${this.apiUrl}/Criticality`);
  }

  public getVulnerability() {
    return this.http.get<number[]>(`${this.apiUrl}/Vulnerability`);
  }

  public getCWE() {
    return this.http.get<number[]>(`${this.apiUrl}/CWE`);
  }

  public getCVSS() {
    return this.http.get<number[]>(`${this.apiUrl}/CVSS`);
  }
}
