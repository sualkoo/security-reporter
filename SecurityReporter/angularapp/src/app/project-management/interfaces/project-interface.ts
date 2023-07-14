export interface ProjectInterface {
  id: string;
  ProjectName?: string;
  StartDate: Date;
  EndDate: Date;
  ProjectStatus?: number;
  ProjectScope?: number;
  ProjectQuestionare?: number;
  PentestAspects?: string;
  PentestDuration?: number;
  ReportDueDate: Date;
  IKO?: Date;
  TKO?: Date;
  RequestCreated?: string;
  Commments: string[];
  CatsNumber?: string;
  ProjectOfferStatus?: number;
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

export const projectStatusIndex: { [key in ProjectStatus]: number } = {
  TBS: 0,
  Requested: 1,
  Planned: 2,
  'In progress': 3,
  Finished: 4,
  Cancelled: 5,
  'On hold': 6,
};

type ProjectQuestionare = 'TBS' | 'Sent' | 'Received';

export const projectQuestionareIndex: { [key in ProjectQuestionare]: number } =
  {
    TBS: 0,
    Sent: 1,
    Received: 2,
  };

type ProjectScope = 'TBS' | 'Sent' | 'Confirmed' | 'Signed';

export const projectScopeIndex: { [key in ProjectScope]: number } = {
  TBS: 0,
  Sent: 1,
  Confirmed: 2,
  Signed: 3,
};

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

export const projectOfferStatusIndex: { [key in ProjectOfferStatus]: number } =
  {
    TBS: -1,
    'Waiting for Offer creation': 0,
    'Offer Draft sent for Review': 1,
    'Offer sent for signatue': 2,
    'Offer signed - Ready For Invoicing': 3,
    Invoiced: 4,
    'Individual Agreement': 5,
    'Retest - free of charge': 6,
    Other: 7,
    Cancelled: 8,
    Prepared: 9,
  };
