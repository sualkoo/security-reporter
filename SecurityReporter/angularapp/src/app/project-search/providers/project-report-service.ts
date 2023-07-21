import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as JSZip from 'jszip';
import { NotificationService } from './notification.service';
import { Observable, delay, tap } from 'rxjs';
import { ProjectDataReport } from '../interfaces/project-data-report.model';
import { PagedResponse } from '../interfaces/paged-response.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectReportService {
  private apiUrl: string;

  constructor(private http: HttpClient, private notificationService: NotificationService) {
    this.apiUrl = 'https://localhost:7075/project-reports';
  }

  public postZipFile(file: any) {
    return this.http.post<ProjectDataReport>(this.apiUrl, file)
  }

  public getProjectReport(id: string) {
    console.log("Fetching project report, id=" + id);
    // Todo: Add type to get request
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  public getProjectReports(subcategory: string, keyword: string, value: string, page: number) {

    return this.http.get<PagedResponse>(this.apiUrl, { params: { subcategory: subcategory, keyword: keyword, value: value, page: page } });
  }


  async validateZipFile(file: File): Promise<boolean> {
    const zip = new JSZip();
    let isValid: boolean = true;

    if (!file) {
      throw new Error('No file specified.');
    }

    const zipData = await zip.loadAsync(file);

    if (Object.keys(zipData.files).length === 0) {
      throw new Error('Zip file cannot be empty.');
    }

    const requiredFiles = {
      configDocumentInformation: zipData.file(
        'Config/Document_Information.tex'
      ),
      configExecutiveSummary: zipData.file('Config/Executive_Summary.tex'),
      configConfigProjectInformation: zipData.file(
        'Config/Project_Information.tex'
      ),
      configScopeAndProcedures: zipData.file('Config/Scope_and_Procedures.tex'),
      configTestingMethodology: zipData.file('Config/Testing_Methodology.tex'),
      configFindingsDatabaseDRTemplate: zipData.file(
        'Config/Findings_Database/DR_Template/main.tex'
      ),
      staticPentestTeam: zipData.file('Static/PentestTeam.tex')
    };

    for (let val of Object.values(requiredFiles)) {
      if (val === null || val === undefined) {
        throw new Error(
          'Zip file has some missing files. Make sure you use the most recent version of the LaTeX template'
        );
      }
    }
    return isValid;
  }
}
