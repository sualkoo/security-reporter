import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as JSZip from 'jszip';

@Injectable({
  providedIn: 'root',
})
export class ProjectDataService {
  private postZipFileUrl: string;

  constructor(private http: HttpClient, private notificationService: NotificationSer) {
    this.postZipFileUrl = 'https://localhost:7075/ProjectReport/add';
  }

  public postZipFile(file: any) {
    return this.http.post(this.postZipFileUrl, file).pipe(res => {
      // error handling 
      return res;
    });
  }

  async validateZipFile(file: File): Promise<boolean> {
    const zip = new JSZip();
    let result: boolean = true;

    if (file && file !== null) {
      try {
        const zipData = await zip.loadAsync(file);

        const requiredFiles = {
          main: zipData.file('Main.tex'),
          configDocumentInformation: zipData.file(
            'Config/Document_Information.tex'
          ),
          configExecutiveSummary: zipData.file('Config/Executive_Summary.tex'),
          configConfigProjectInformation: zipData.file(
            'Config/Project_Information.tex'
          ),
          configScopeAndProcedures: zipData.file(
            'Config/Scope_and_Procedures.tex'
          ),
          configTestingMethodology: zipData.file(
            'Config/Testing_Methodology.tex'
          ),
          configFindingsDatabase: zipData.file(
            'Config/Findings_Database/Findings_Database.tex'
          ),
          configFindingsDatabaseDRTemplate: zipData.file(
            'Config/Findings_Database/DR_Template/main.tex'
          ),
          configTestProtocolHowTo: zipData.file(
            'Config/Test_Protocol/HowTo.txt'
          ),
          configTestProtocolOwaspCSV: zipData.file(
            'Config/Test_Protocol/owasp.csv'
          ),
          configTestProtocolOwaspXLSX: zipData.file(
            'Config/Test_Protocol/owasp.xlsx'
          ),
        };

        for (let val of Object.values(requiredFiles)) {
          if (val === null || val === undefined) {
            result = false;
          }
        }
        return result;
      } catch (error) {
        console.error('Error reading the zip file: ', error);
      }
    }

    return result;
  }
}
