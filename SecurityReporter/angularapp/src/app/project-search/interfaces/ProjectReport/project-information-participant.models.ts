import { ProjectParticipantRole } from "../Enums/project-participant-role.enum";

export interface ProjectInformationParticipant {
  name?: string;
  role: ProjectParticipantRole;
  department?: string;
  contact?: string;
}
