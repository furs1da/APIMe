export interface UserForRegistrationDto {
  firstName: string;
  lastName: string;
  email: string;
  studentSection: number;
  studentNumber: number;
  accessCode: string;
  password: string;
  confirmPassword: string;
}
