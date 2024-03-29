import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Section } from '../../../interfaces/request/section';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { ProfessorProfile } from 'src/interfaces/profile/profile/professorProfile';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { Property } from '../../../interfaces/response/property';
import { TestRouteResponse } from 'src/interfaces/response/testRouteResponse';
import { Student } from '../../../interfaces/request/student';
import { StudentProfile } from '../../../interfaces/profile/profile/studentProfile';
import { AuthResponseDto } from '../../../interfaces/response/authResponseDto';
import { RouteLogDto } from '../../../interfaces/routelog/routeLogDTO';

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
    return this.http.post<Section>(this.createCompleteRoute('sectionApi/add', this.baseUrl), section);
  }

  public updateSection(section: Section) {
    return this.http.put<Section>(this.createCompleteRoute('sectionApi/edit/' + section.id, this.baseUrl), section);
  }


  public deleteSection(id: number) {
    return this.http.delete(this.createCompleteRoute(`sectionApi/delete/${id}`, this.baseUrl));
  }



  getStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.createCompleteRoute(`studentApi/students`, this.baseUrl))
      .pipe(catchError(this.handleError<Student[]>('getStudents', [])));
  }

  createStudent(student: Student): Observable<Student> {
    return this.http.post<Student>(this.createCompleteRoute(`studentApi/add`, this.baseUrl), student);
  }

  updateStudent(student: Student): Observable<Student> {
    return this.http.put<Student>(this.createCompleteRoute(`studentApi/edit`, this.baseUrl), student);
  }

  getStudent(id: number): Observable<Student> {
    return this.http.get<Student>(this.createCompleteRoute(`studentApi/student/` + id, this.baseUrl))
      ;
  }

  deleteStudent(id: number): Observable<unknown> {
    return this.http
      .delete(this.createCompleteRoute(`studentApi/delete/` + id, this.baseUrl))
      .pipe(catchError(this.handleError('deleteStudent')));
  }


  getSectionsStudent(): Observable<Section[]> {
    return this.http
      .get<Section[]>(this.createCompleteRoute(`studentApi/sections`, this.baseUrl))
      .pipe(catchError(this.handleError<Section[]>('getSectionsStudent', [])));;
  }














  public getRoutes = () => {
    return this.http.get<RouteDto[]>(this.createCompleteRoute('routeApi/routes', this.baseUrl));
  }

  public createRoute(route: RouteDto) {
    return this.http.post<RouteDto>(this.createCompleteRoute('routeApi/add', this.baseUrl), route);
  }

  public updateRoute(route: RouteDto) {
    return this.http.put<RouteDto>(this.createCompleteRoute(`routeApi/update/${route.id}`, this.baseUrl), route);
  }

  public deleteRoute(id: number) {
    return this.http.delete(this.createCompleteRoute(`routeApi/delete/${id}`, this.baseUrl));
  }


  public getPropertiesByRouteId(routeId: number): Observable<Property[]> {
    return this.http.get<Property[]>(this.createCompleteRoute(`routeApi/properties/${routeId}`, this.baseUrl));
  }

  public getPropertiesByTableName(tableName: string): Observable<Property[]> {
    return this.http.get<Property[]>(this.createCompleteRoute(`routeApi/properties/byTableName/${tableName}`, this.baseUrl));
  }









  public testRoute(routeId: number, values: any): Observable<TestRouteResponse> {
    return this.http.post<TestRouteResponse>(
      this.createCompleteRoute(`routeApi/testRoute/${routeId}`, this.baseUrl),
      values
    );
  }


  public getClaims = (route: string) => {
    return this.http.get(this.createCompleteRoute(route, this.baseUrl));
  }
  
  public getProfessorProfile = (route: string) => {
    return this.http.get<ProfessorProfile>(this.createCompleteRoute(route, this.baseUrl));
  }
  public getStudentProfile = (route: string) => {
    return this.http.get<StudentProfile>(this.createCompleteRoute(route, this.baseUrl));
  }




  public updateProfessorProfile(professorProfile: ProfessorProfile): Observable<AuthResponseDto> {
    return this.http.put<AuthResponseDto>(this.createCompleteRoute('profileApi/edit/' + professorProfile.id, this.baseUrl), professorProfile);
  }


  public updateStudentProfile(studentProfile: StudentProfile): Observable<AuthResponseDto> {
    return this.http.put<AuthResponseDto>(this.createCompleteRoute('profileApi/editStudent/' + studentProfile.id, this.baseUrl), studentProfile);
  }



  public getAllRouteLogs(): Observable<RouteLogDto[]> {
    return this.http.get<RouteLogDto[]>(this.createCompleteRoute('routeLogApi/logs', this.baseUrl));
  }

  public getRouteLogsByUserId(userId: string): Observable<RouteLogDto[]> {
    return this.http.get<RouteLogDto[]>(this.createCompleteRoute(`routeLogApi/logs/byuser/${userId}`, this.baseUrl));
  }

  public clearAllRouteLogs(): Observable<any> {
    return this.http.delete(this.createCompleteRoute('routeLogApi/clear', this.baseUrl));
  }



  
  private createCompleteRoute = (route: string, baseUrl: string) => {
    return `${baseUrl}${route}`;
  }



  public get(route: string, options?: { headers?: HttpHeaders }): Observable<any> {
    return this.http.get(route, options);
  }

  public post(route: string, body: any, options?: { headers?: HttpHeaders }): Observable<any> {
    return this.http.post(route, body, options);
  }

  public put(route: string, body: any, options?: { headers?: HttpHeaders }): Observable<any> {
    return this.http.put(route, body, options);
  }

  public patch(route: string, body: any, options?: { headers?: HttpHeaders }): Observable<any> {
    return this.http.patch(route, body, options);
  }

  public delete(route: string, options?: { headers?: HttpHeaders }): Observable<any> {
    return this.http.delete(route, options);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}
