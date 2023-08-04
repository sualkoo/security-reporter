import { ReportVersionEntry } from "./report-version-entry.models";

export interface DocumentInformation {
  projectReportName?: string;
  assetType?: string;
  mainAuthor?: string;
  authors?: Array<string>;
  reviewiers?: Array<string>;
  approvers?: Array<string>;
  reportDocumentHistory?: Array<ReportVersionEntry>;
  reportDate: Date; //(obsahuje aj casovu hodinu? - nie je dateOnly)
  
}
