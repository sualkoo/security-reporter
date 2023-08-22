import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import * as JSZip from 'jszip';
import { ProjectReport } from '../interfaces/project-report.model';
import { PagedResponse } from '../interfaces/paged-response.model';
import { FindingResponse } from '../interfaces/finding-response.model';
import {ApiPaths} from "../../api-paths.enum";

@Injectable({
  providedIn: 'root',
})
export class ProjectReportService {

  public readonly apiUri: string;

  constructor(private http: HttpClient) {
    this.apiUri = ApiPaths.ProjectReports;
  }

  public postZipFile(file: any) {
    return this.http.post<ProjectReport>(this.apiUri, file)
  }

  public getProjectReport(id: string) {
    return this.http.get(`${this.apiUri}/${id}`);
  }

  public getProjectReportFindings(page: number, projectName?: string, details?: string, impact?: string, repeatability?: string, references?: string, cwe?: string, findingName?: string) {
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

    if (findingName !== undefined) {
      params = params.set('findingName', findingName);
    }

    return this.http.get<PagedResponse<FindingResponse>>(`${this.apiUri}/findings`, { params: params });
  }

  public deleteProjectReport(ids: string[]) {
    return this.http.delete<string[]>(this.apiUri,  { body: ids });
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
