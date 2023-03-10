import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { AuthenticationService } from './../../shared/services/authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserForRegistrationDto } from '../../../interfaces/user/userForRegistrationDTO';
import { Section } from '../../../interfaces/request/section';
import { PasswordConfirmationValidatorService } from '../../shared/custom-validators/password-confirmation-validator.service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {

  registerForm: FormGroup = new FormGroup({});
  sectionList: Section[] = [];
  errorMessage: string = '';
  showError: boolean = false;

  constructor(private authService: AuthenticationService, private passConfValidator: PasswordConfirmationValidatorService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http.get<{ sectionList: Section[] }>(baseUrl + 'account/sectionList')
      .subscribe(data => {
        this.sectionList = data.sectionList;
        console.log(this.sectionList);
      });
  }


  ngOnInit(): void {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      accessCode: new FormControl('', [Validators.required]),
      studentSection: new FormControl('', [Validators.required]),
      studentNumber: new FormControl('', [Validators.required, Validators.minLength(7), Validators.maxLength(7)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
    });

    const confirmControl = this.registerForm.get('confirm');
    const passwordControl = this.registerForm.get('password');

    if (passwordControl && confirmControl) {
      confirmControl.setValidators([Validators.required,
        this.passConfValidator.validateConfirmPassword(passwordControl)]);
    }
   
  }


  public validateControl = (controlName: string) => {
    const control = this.registerForm && this.registerForm.get(controlName);
    return control && control.invalid && control.touched;
  }

  public hasError = (controlName: string, errorName: string) => {
    const control = this.registerForm && this.registerForm.get(controlName);
    return control && control.hasError(errorName);
  }


  public registerUser = (registerFormValue: any) => {
    this.showError = false;
    const formValues = { ...registerFormValue };
    const user: UserForRegistrationDto = {
      firstName: formValues.firstName,
      lastName: formValues.lastName,
      email: formValues.email,
      studentSection: formValues.studentSection,
      studentNumber: formValues.studentNumber,
      accessCode: formValues.accessCode,
      password: formValues.password,
      confirmPassword: formValues.confirm
    };

    this.authService.registerUser("account/registration", user)
      .subscribe({
        next: (_) => console.log("Successful registration"),
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
  }
}