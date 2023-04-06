import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from './../../shared/services/authentication.service';

import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserForAuthenticationDto } from '../../../interfaces/user/userForAuthenticationDto';
import { AuthResponseDto } from '../../../interfaces/response/authResponseDto';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private returnUrl: string = "";

  loginForm: FormGroup = new FormGroup({});
  errorMessage: string = '';
  showError: boolean = false;
  constructor(private authService: AuthenticationService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }


  validateControl = (controlName: string) => {
    const control = this.loginForm && this.loginForm.get(controlName);
    return control && control.invalid && control.touched;
  }

  hasError = (controlName: string, errorName: string) => {
    const control = this.loginForm && this.loginForm.get(controlName);
    return control && control.hasError(errorName);
  }



  loginUser = (loginFormValue: any) => {
    this.showError = false;
    const login = { ...loginFormValue };
    const userForAuth: UserForAuthenticationDto = {
      email: login.username,
      password: login.password
    }

    this.authService.loginUser('account/login', userForAuth)
      .subscribe({
        next: (res: AuthResponseDto) => {
          localStorage.setItem("token", res.token);
          this.authService.sendAuthStateChangeNotification(res.isAuthSuccessful);
          this.router.navigate([this.returnUrl]);
        },
        error: (err: HttpErrorResponse) => {
          console.log(err);
          this.errorMessage = err.error.message;
          this.showError = true;
        }
      })
  }
}
