import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Section } from '../../../interfaces/request/section';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

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

  public async getSectionsAdmin(route: string): Promise<Section[]> {
    const response = await this.http.get<Section[]>(this.createCompleteRoute(route, this.baseUrl)).toPromise();
    return response ? response : [];
  }

  public createSection(section: Section) {
    console.log(this.createCompleteRoute('sectionApi/add', this.baseUrl));
    return this.http.post<Section>(this.createCompleteRoute('sectionApi/add', this.baseUrl), section);
  }

  public updateSection(section: Section) {
    return this.http.put<Section>(this.createCompleteRoute('sectionApi/edit/' + section.id, this.baseUrl), section);
  }


  public getClaims = (route: string) => {
    return this.http.get(this.createCompleteRoute(route, this.baseUrl));
  }

  private createCompleteRoute = (route: string, baseUrl: string) => {
    console.log(`${baseUrl}${route}`);
    return `${baseUrl}${route}`;
  }
}
