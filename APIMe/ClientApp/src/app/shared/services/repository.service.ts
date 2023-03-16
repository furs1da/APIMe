import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Section } from '../../../interfaces/request/section';


@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  baseUrl: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  public getSections = (route: string) => {
    return this.http.get<Section[]>(this.createCompleteRoute(route, this.baseUrl));
  }

  public getClaims = (route: string) => {
    return this.http.get(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, baseUrl: string) => {
    console.log(`${baseUrl}${route}`);
    return `${baseUrl}${route}`;
  }
}
