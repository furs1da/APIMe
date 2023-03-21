import { Section } from "../interfaces/request/section";

export class SectionClass implements Section {
    id: number;
    sectionName: string;
    professorName: string;
    accessCode: string;

    constructor(section: Section) {
      this.id = section.id;
      this.sectionName = section.sectionName;
      this.professorName = section.professorName;
      this.accessCode = section.accessCode;
    }
}





