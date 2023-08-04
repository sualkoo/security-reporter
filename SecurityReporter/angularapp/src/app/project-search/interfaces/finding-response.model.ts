import { Finding } from "./ProjectReport/finding";

export interface FindingResponse {
  projectReportId: string;
  projectReportName: string;
  finding: Finding;
}
