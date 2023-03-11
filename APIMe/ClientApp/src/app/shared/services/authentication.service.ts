import { Inject, Injectable } from '@angular/core';
import { UserForRegistrationDto } from '../../../interfaces/user/userForRegistrationDTO';
import { RegistrationResponseDto } from '../../../interfaces/response/registrationResponseDTO';
import { HttpClient } from '@angular/common/http';
import { UserForAuthenticationDto } from '../../../interfaces/user/userForAuthenticationDto';
import { AuthResponseDto } from '../../../interfaces/response/authResponseDto';
import { Subject } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl: string = "";

  private authChangeSub = new Subject<boolean>()
  public authChanged = this.authChangeSub.asObservable();

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    return this.http.post<RegistrationResponseDto>(this.createCompleteRoute(route, this.baseUrl), body);
  }

  public loginUser = (route: string, body: UserForAuthenticationDto) => {
    return this.http.post<AuthResponseDto>(this.createCompleteRoute(route, this.baseUrl), body);
  }

  private createCompleteRoute = (route: string, baseUrl: string) => {
    return `${baseUrl}${route}`;
  }

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }

  public logout = () => {
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
  }
}
