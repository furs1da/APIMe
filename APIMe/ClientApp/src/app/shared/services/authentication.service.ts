import { Inject, Injectable } from '@angular/core';
import { UserForRegistrationDto } from '../../../interfaces/user/userForRegistrationDTO';
import { RegistrationResponseDto } from '../../../interfaces/response/registrationResponseDTO';
import { HttpClient } from '@angular/common/http';




@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public registerUser = (route: string, body: UserForRegistrationDto) => {
    return this.http.post<RegistrationResponseDto>(this.createCompleteRoute(route, this.baseUrl), body);
  }

  private createCompleteRoute = (route: string, baseUrl: string) => {
    return `${baseUrl}${route}`;
  }
}
