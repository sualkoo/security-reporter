import { Tool } from "./tool.models";


export interface TestingMethodology {
  toolsUsed?: Array<Tool>;
  attackVectors?: Array<string>;
}
