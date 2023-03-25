import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RouteTypeDto } from '../../../interfaces/response/routeTypeDTO';

@Injectable({
  providedIn: 'root'
})
export class RouteTypesService {
  private readonly baseUrl: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getRouteTypes(): Observable<RouteTypeDto[]> {
    return this.http.get<RouteTypeDto[]>(this.createCompleteRoute('routeTypeApi/routes', this.baseUrl));
  }

  private createCompleteRoute = (route: string, baseUrl: string) => {
    console.log(`${baseUrl}${route}`);
    return `${baseUrl}${route}`;
  }
}
