import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl: string;
  constructor(private http: HttpClient) {
    this.apiUrl = 'https://localhost:7075/dashboard';
}

  public getCriticality() {
    return this.http.get<number[]>(`${this.apiUrl}/Criticality`);
  }
}
