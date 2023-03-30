import { Section } from "./section";

export interface Student {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  studentId: number;
  apiKey: string;
  sections: Section[];
}
