import * as JSZip from 'jszip';

export class ZipValidator {

  async validate(file: File): Promise<boolean> {
    const zip = new JSZip();

    if (file && file !== null) {
      try {
        const zipData = await zip.loadAsync(file);

        const main = zipData.file('Main.tex');
        const configDocumentInformation = zipData.file('Config/Document_Information.tex');
        const configExecutiveSummary = zipData.file('Config/Executive_Summary.tex');
        const configConfigProjectInformation = zipData.file('Config/Project_Information.tex');
        const configScopeAndProcedures = zipData.file('Config/Scope_and_Procedures.tex');
        const configTestingMethodology = zipData.file('Config/Testing_Methodology.tex');
        const configFindingsDatabase = zipData.file('Config/Findings_Database/Findings_Database.tex');
        const configFindingsDatabaseDRTemplate = zipData.file('Config/Findings_Database/DR_Template/main.tex');
        const configTestProtocolHowTo = zipData.file('Config/Test_Protocol/HowTo.txt');
        const configTestProtocolOwaspCSV = zipData.file('Config/Test_Protocol/owasp.csv');
        const configTestProtocolOwaspXLSX = zipData.file('Config/Test_Protocol/owasp.xlsx');

        if (main !== null && configDocumentInformation !== null && configExecutiveSummary !== null && configConfigProjectInformation !== null && configScopeAndProcedures !== null
          && configTestingMethodology !== null && configFindingsDatabase !== null && configFindingsDatabaseDRTemplate !== null && configTestProtocolHowTo !== null
          && configTestProtocolOwaspCSV !== null && configTestProtocolOwaspXLSX !== null) {
          return true;
        } else {
          return false;
        }
      } catch (error) {
        console.error("Error reading the zip file: ", error)
      }
  }

    return false;
  }
}
