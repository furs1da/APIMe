import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from './../../shared/services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ForgotPasswordDto } from '../../../interfaces/request/forgotPassword';
@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup = new FormGroup({});
  successMessage: string = "";
  errorMessage: string = "";
  showSuccess: boolean = false;
  showError: boolean = false;

  constructor(private _authService: AuthenticationService) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl("", [Validators.required])
    })
  }

  public validateControl = (controlName: string) => {
    const control = this.forgotPasswordForm && this.forgotPasswordForm.get(controlName);
    return control && control.invalid && control.touched;
  }

  public hasError = (controlName: string, errorName: string) => {
    const control = this.forgotPasswordForm && this.forgotPasswordForm.get(controlName);
    return control && control.hasError(errorName);
  }


  public forgotPassword = (forgotPasswordFormValue : any) => {
    this.showError = this.showSuccess = false;
    const forgotPass = { ...forgotPasswordFormValue };
    const forgotPassDto: ForgotPasswordDto = {
      email: forgotPass.email,
      clientURI: 'http://localhost:' + window.location.port + '/authentication/resetpassword'
    }
    this._authService.forgotPassword('account/forgotpassword', forgotPassDto)
      .subscribe({
        next: (_) => {
          this.showSuccess = true;
          this.successMessage = 'The link has been sent, please check your email to reset your password.'
        },
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.message;
        }
      })
  }
}
