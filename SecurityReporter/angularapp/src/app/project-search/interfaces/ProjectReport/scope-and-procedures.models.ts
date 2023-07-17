import { ScopeProcedure } from "./scope-procedure.models";

export interface ScopeAndProcedures {
  inScope?: Array<ScopeProcedure>;
  outOfScope?: Array<ScopeProcedure>;
  worstCaseScenarios?: Array<string>;
  description?: Array<string>;
}
