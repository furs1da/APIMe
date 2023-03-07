export interface UserForRegistrationDto {
  firstName: string;
  lastName: string;
  email: string;
  studentSection: number;
  accessCode: string;
  password: string;
  confirmPassword: string;
}
