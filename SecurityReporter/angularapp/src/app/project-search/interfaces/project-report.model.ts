import { DocumentInformation } from "./ProjectReport/document-information.models";
import { Finding } from "./ProjectReport/finding";
import { ProjectInformation } from "./ProjectReport/project-information.models";
import { ScopeAndProcedures } from "./ProjectReport/scope-and-procedures.models";
import { TestingMethodology } from "./ProjectReport/testing-methodology.models";

export interface ProjectReport
{
  id: string;
  documentInfo?: DocumentInformation;
  executiveSummary?: string;
  projectInfo?: ProjectInformation;
  findings?: Array<Finding>;
  scopeAndProcedures?: ScopeAndProcedures;
  testingMethodology?: TestingMethodology;
}
