export interface ProjectData {
  ProjectName?: string;
  ProjectStatus?: number;
  ProjectQuestionare?: number;
  ProjectScope?: number;
  PentestStart?: number;
  PentestEnd?: number;
  StartDate?: Date;
  EndDate?: Date;
  IKO?: Date;
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
  Requested: 0,
  Planned: 1,
  'In progress': 2,
  Finished: 3,
  Cancelled: 4,
  'On hold': 5,
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
