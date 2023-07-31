import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import * as JSZip from 'jszip';
import { NotificationService } from './notification.service';
import { ProjectReport } from '../interfaces/project-report.model';
import { PagedResponse } from '../interfaces/paged-response.model';
import { FindingResponse } from '../interfaces/finding-response.model';

@Injectable({
  providedIn: 'root',
})
export class ProjectReportService {
  private apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = 'https://localhost:7075/project-reports';
  }

  public postZipFile(file: any) {
    return this.http.post<ProjectReport>(this.apiUrl, file)
  }

  public getProjectReport(id: string) {
    console.log("Fetching project report, id=" + id);
    // Todo: Add type to get request
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  public getProjectReportFindings(page: number, projectName?: string, details?: string, impact?: string, repeatability?: string, references?: string, cwe?: string) {
    let params = new HttpParams();

    params = params.set('page', page);

    console.log("Project name: " + projectName);

    if (projectName !== undefined) {
      params = params.set('projectName', projectName);
    }

    if (details !== undefined) {
      params = params.set('details', details);
    }

    if (impact !== undefined) {
      params = params.set('impact', impact);
    }

    if (repeatability !== undefined) {
      params = params.set('repeatability', repeatability);
    }

    if (references !== undefined) {
      params = params.set('references', references);
    }

    if (cwe !== undefined) {
      params = params.set('cwe', cwe);
    }

    console.log(params);
    return this.http.get<PagedResponse<FindingResponse>>(`${this.apiUrl}/findings`, { params: params });
  }

  /*public getProjectReports(subcategory: string, keyword: string, value: string, page: number) {

    return this.http.get<PagedResponse>(this.apiUrl, { params: { subcategory: subcategory, keyword: keyword, value: value, page: page } });
  }*/

  public deleteProjectReport(ids: string[]) {
    console.log(ids);
    return this.http.delete<string[]>(this.apiUrl,  { body: ids });
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
