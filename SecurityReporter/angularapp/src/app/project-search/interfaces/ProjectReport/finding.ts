import { Category } from "../Enums/category.enum";
import { Criticality } from "../Enums/criticality.enum";
import { Detectability } from "../Enums/detectability.enum";
import { Exploitability } from "../Enums/exploitability.enum";

export interface Finding {
  findingAuthor?: string;
  findingName?: string;
  location?: Array<string>;
  component?: string;
  foundWith?: string;
  testMethod?: string;
  cvss?: string;
  cvssVector?: string;
  cwe?: string;
  criticality: Criticality;
  exploitability: Exploitability;
  category: Category;
  detectability: Detectability;
  subsectionDetails?: string;
  subsectionImpact?: string;
  subsectionRepeatability?: string;
  subsectionCountermeasures?: string;
  subsectionReferences?: Array<string>;
}
