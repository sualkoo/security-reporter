export interface ProjectInterface {
  ProjectName?: string;
  StartDate?: Date;
  EndDate?: Date;
  ProjectStatus?: ProjectStatus;
  ProjectScope?: ProjectScope;
  ProjectQuestionare?: ProjectQuestionare;
  PentestAspects?: string;
  PentestDuration?: string;
  ReportDueDate?: Date;
  IKO?: Date;
  TKO?: Date;
  RequestCreated?: string;
  Commments?: string;
  CatsNumber?: string;
  ProjectOfferStatus?: ProjectOfferStatus;
  WorkingTeam: string[];
  ProjectLead?: string;
  ReportStatus?: string;
  ContactForClients: string[];
}

type ProjectStatus =
  | 'TBS'
  | 'Requested'
  | 'Planned'
  | 'In progress'
  | 'Finished'
  | 'Cancelled'
  | 'On hold';

type ProjectQuestionare = 'TBS' | 'Sent' | 'Received';

type ProjectScope = 'TBS' | 'Sent' | 'Confirmed' | 'Signed';

type ProjectOfferStatus =
  | 'TBS'
  | 'Waiting for Offer creation'
  | 'Offer Draft sent for Review'
  | 'Offer sent for signatue'
  | 'Offer signed - Ready For Invoicing'
  | 'Invoiced'
  | 'Individual Agreement'
  | 'Retest - free of charge'
  | 'Other'
  | 'Cancelled'
  | 'Prepared';
