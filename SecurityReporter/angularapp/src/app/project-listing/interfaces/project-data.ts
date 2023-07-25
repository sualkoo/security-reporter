export interface ProjectData {
  ProjectName?: string;
  ProjectStatus?: number;
  Questionare?: number;
  ProjectScope?: number;
  PentestStart?: number;
  PentestEnd?: number;
  StartDate?: Date;
  EndDate?: Date;
  IKO?: number;
  TKO?: number;
}

type ProjectStatus =
  | 'Requested'
  | 'Planned'
  | 'In progress'
  | 'Finished'
  | 'Cancelled'
  | 'On hold';

export const projectStatusIndex: { [key in ProjectStatus]: number } = {
  Requested: 1,
  Planned: 2,
  'In progress': 3,
  Finished: 4,
  Cancelled: 5,
  'On hold': 6,
};

type Questionare = 'TBS' | 'Sent' | 'Received';

export const QuestionareIndex: { [key in Questionare]: number } =
{
  TBS: 1,
  Sent: 2,
  Received: 3,
};

type ProjectScope = 'TBS' | 'Sent' | 'Confirmed' | 'Signed';

export const projectScopeIndex: { [key in ProjectScope]: number } = {
  TBS: 0,
  Sent: 1,
  Confirmed: 2,
  Signed: 3,
};

type IKO = 'TBD' | 'Date is set';

export const IKOIndex: { [key in IKO]: number } = {
  TBD: 0,
  "Date is set": 1,
};

