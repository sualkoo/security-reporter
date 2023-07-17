import { ProjectInformationParticipant } from "./project-information-participant.models";

export interface ProjectInformation {
  applicationManager?: ProjectInformationParticipant;
  businessOwner?: ProjectInformationParticipant;
  businessRepresentative?: ProjectInformationParticipant;
  technicalContacts?: Array<ProjectInformationParticipant>;
  pentestLead?: ProjectInformationParticipant;
  pentestCoordinator?: ProjectInformationParticipant;
  pentestTeam?: Array<ProjectInformationParticipant>;
  targetInfoVersion?: string;
  targetInfoEnviroment?: string;
  targetInfoInternetFacing: boolean;
  targetInfoSNXConectivity: boolean;
  targetInfoHostingConnection?: string;
  targetInfoHostingProvider?: string;
  targetInfoLifeCyclePhase?: string;
  targetInfoCriticality?: string;
  targetInfoAssetID?: string;
  targetInfoSHARPUUID?: string;
  targetInfoDescription?: string;
  timeFrameTotal: number;
  timeFrameStart: Date;
  timeFrameEnd: Date;
  timeFrameReportDue: Date;
  timeFrameComment?: string;
  findingsCountCriticalTBD: number;
  findingsCountCritical: number;
  findingsCountCriticalHigh: number;
  findingsCountCriticalMedium: number;
  findingsCountCriticalLow: number;
  findingsCountCriticalInfo: number;
  findingsCountCriticalTotal: number;
}
