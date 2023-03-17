import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ResetPasswordDto } from '../../../interfaces/request/resetPassword';
import { PasswordConfirmationValidatorService } from '../../shared/custom-validators/password-confirmation-validator.service';
import { AuthenticationService } from '../../shared/services/authentication.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup = new FormGroup({});
  showSuccess: boolean = false;
  showError: boolean = false;
  errorMessage: string = "";
  private token: string = "";
  private email: string = "";

  constructor(private authService: AuthenticationService, private passConfValidator: PasswordConfirmationValidatorService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
    });

    const passwordControl = this.resetPasswordForm.get('password');
    const confirmControl = this.resetPasswordForm.get('confirm');


    if (passwordControl && confirmControl) {
      confirmControl.setValidators([Validators.required,
        this.passConfValidator.validateConfirmPassword(passwordControl)]);
    }
    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
  }

  public validateControl = (controlName: string) => {
    const control = this.resetPasswordForm && this.resetPasswordForm.get(controlName);
    return control && control.invalid && control.touched;
  }


  public hasError = (controlName: string, errorName: string) => {
    const control = this.resetPasswordForm && this.resetPasswordForm.get(controlName);
    return control && control.hasError(errorName);
  }


  public resetPassword = (resetPasswordFormValue : any) => {
    this.showError = this.showSuccess = false;
    const resetPass = { ...resetPasswordFormValue };
    const resetPassDto: ResetPasswordDto = {
      password: resetPass.password,
      confirmPassword: resetPass.confirm,
      token: this.token,
      email: this.email
    }
    this.authService.resetPassword('account/resetpassword', resetPassDto)
      .subscribe({
        next: (_) => this.showSuccess = true,
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.message;
        }
      })
  }
}
