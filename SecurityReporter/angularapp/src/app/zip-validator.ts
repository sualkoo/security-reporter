import * as JSZip from 'jszip';

export class ZipValidator {

  //tu bude funkcia na kontrolu, ci je zip validne
  async validate(file: File): Promise<boolean> {
    const zip = new JSZip();

    try {
      const zipData = await zip.loadAsync(file);

      const Main = zipData.file('Main.tex');
      const ConfigDocumentInformation = zipData.file('Config/Document_Information.tex');
      const ConfigExecutiveSummary = zipData.file('Config/Executive_Summary.tex');
      const ConfigConfigProjectInformation = zipData.file('Config/Project_Information.tex');
      const ConfigScopeAndProcedures = zipData.file('Config/Scope_and_Procedures.tex');
      const ConfigTestingMethodology = zipData.file('Config/Testing_Methodology.tex');
      const ConfigFindingsDatabase = zipData.file('Config/Findings_Database/Findings_Database.tex');
      const ConfigFindingsDatabaseDRTemplate = zipData.file('Config/Findings_Database/DR_Template/main.tex');
      const ConfigTestProtocolHowTo = zipData.file('Config/Test_Protocol/HowTo.txt');
      const ConfigTestProtocolOwaspCSV = zipData.file('Config/Test_Protocol/owasp.csv');
      const ConfigTestProtocolOwaspXLSX = zipData.file('Config/Test_Protocol/owasp.xlsx');

      if (Main !== null && ConfigDocumentInformation !== null && ConfigExecutiveSummary !== null && ConfigConfigProjectInformation !== null && ConfigScopeAndProcedures !== null
        && ConfigTestingMethodology !== null && ConfigFindingsDatabase !== null && ConfigFindingsDatabaseDRTemplate !== null && ConfigTestProtocolHowTo !== null
        && ConfigTestProtocolOwaspCSV !== null && ConfigTestProtocolOwaspXLSX !== null) {
        console.log("ok");
        return true;
      } else {
        console.log("folder je null");
        return false;
      }
    } catch (error) {
      console.error("Error readint the zip file: ", error)
    }

    return false;
  }
}
