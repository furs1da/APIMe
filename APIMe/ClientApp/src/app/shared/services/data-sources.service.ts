import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DataSourceDto } from '../../../interfaces/response/dataSourceDTO';


@Injectable({
  providedIn: 'root'
})
export class DataSourcesService {
  baseUrl: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  getDataSources(): Observable<DataSourceDto[]> {
    return this.http.get<DataSourceDto[]>(this.createCompleteRoute('routeApi/dataSources', this.baseUrl));
  }


  private createCompleteRoute = (route: string, baseUrl: string) => {
    console.log(`${baseUrl}${route}`);
    return `${baseUrl}${route}`;
  }
}
