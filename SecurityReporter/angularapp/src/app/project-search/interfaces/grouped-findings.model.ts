import { Finding } from "./ProjectReport/finding";

export interface GroupedFinding {
  projectId: string;
  projectName: string;
  findings: Finding[];
  checked: boolean
}
