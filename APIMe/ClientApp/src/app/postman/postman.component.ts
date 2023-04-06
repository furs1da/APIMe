import { HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Property } from '../../interfaces/response/property';
import { DataSourcesService } from '../shared/services/data-sources.service';
import { RepositoryService } from '../shared/services/repository.service';
import { MatTableDataSource } from '@angular/material/table';
import { fromEvent } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';


@Component({
  selector: 'app-postman',
  templateUrl: './postman.component.html',
  styleUrls: ['./postman.component.css']
})
export class PostmanComponent implements OnInit {
  requestTypes: string[] = ['GET', 'POST', 'PUT', 'PATCH', 'DELETE'];
  requestType: string = 'GET';
  endpoint: string = '';
  requestBody: string = '';
  public headers: { [headerKey: string]: string } = {};
  tableNames: string[] = [];
  baseUrl: string = "";
  errorMessage: string | null = null;

  response: MatTableDataSource<any> | undefined;
  columns: string[] = [];




  constructor(private repositoryService: RepositoryService, private dataSourcesService: DataSourcesService, @Inject('BASE_URL') baseUrl: string,) {
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.dataSourcesService.getDataSources().subscribe(
      (data) => {
        this.tableNames = data.map((dataSource) => dataSource.name);
      },
      (error) => {
        console.error('Error:', error);
        if (error.error && typeof error.error === 'object') {
          this.errorMessage = 'Error: ' + JSON.stringify(error.error);
        } else {
          this.errorMessage = 'Error: ' + error.message;
        }
      }
    );
  }


  ngAfterViewInit(): void {
    const endpointInput = document.querySelector('input[matInput]');
    if (endpointInput) {
      fromEvent(endpointInput, 'input')
        .pipe(debounceTime(1000))
        .subscribe(() => {
          this.generateRequestBody();
        });
    }
  }


  isRequestBodyRequired(): boolean {
    return this.requestType === 'POST' || this.requestType === 'PUT' || this.requestType === 'PATCH';
  }

  getPropertyValue(element: any[], column: string): any {
    const property = element.find((property: Property) => property.name === column);
    return property ? property.value : null;
  }


  suggestEndpoint(): void {
    if (this.tableNames.length > 0) {
      const randomTable = this.tableNames[Math.floor(Math.random() * this.tableNames.length)];
      this.endpoint = `${this.baseUrl}routeApi/records/${randomTable.toLowerCase()}`;

      if (this.requestType === 'DELETE' || this.requestType === 'PATCH') {
        const randomId = Math.floor(Math.random() * 100) + 1; // Random ID from 1 to 100
        this.endpoint += `/${randomId}`;
      }
    } else {
      this.endpoint = `${this.baseUrl}routeApi/records/orders`;
      if (this.requestType === 'DELETE' || this.requestType === 'PATCH') {
        const randomId = Math.floor(Math.random() * 100) + 1; // Random ID from 1 to 100
        this.endpoint += `/${randomId}`;
      }
    }
  }


  addHeader(): void {
    let newKey = `Header-${Object.keys(this.headers).length + 1}`;
    while (this.headers.hasOwnProperty(newKey)) {
      newKey = `Header-${parseInt(newKey.split('-')[1]) + 1}`;
    }
    this.headers[newKey] = '';
  }


    removeHeader(key: string): void {
      delete this.headers[key];
    }

    headerKeys(): string[] {
      return Object.keys(this.headers);
    }

  // Modify the sendRequest method in the PostmanComponent class:

  sendRequest(): void {
    this.errorMessage = null;

    const headers = new HttpHeaders(this.headers);
    switch (this.requestType) {
      case 'GET':
        this.repositoryService
          .get(this.endpoint, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              const transformedResponse = this.transformResponse(response);
              this.response = new MatTableDataSource(transformedResponse);
              if (transformedResponse.length > 0) {
                this.columns = Object.keys(transformedResponse[0]);
              }
            },
            (error) => {
              console.error('Error:', error);
              if (error.error && error.error.message) {
                this.errorMessage = 'Error: ' + error.error.message;
              } else if (error.error && typeof error.error === 'object') {
                this.errorMessage = 'Error: ' + JSON.stringify(error.error, null, 2);
              } else {
                this.errorMessage = 'Error: ' + error.message;
              }
            }
          );
        break;
      case 'POST':
        this.repositoryService
          .post(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              const transformedResponse = this.transformResponse(response);
              this.response = new MatTableDataSource(transformedResponse);
              if (transformedResponse.length > 0) {
                this.columns = Object.keys(transformedResponse[0]);
              }
            },
            (error) => {
              console.error('Error:', error);
              if (error.error && typeof error.error === 'object') {
                this.errorMessage = 'Error: ' + JSON.stringify(error.error);
              } else {
                this.errorMessage = 'Error: ' + error.message;
              }
            }
          );
        break;
      case 'PUT':
        this.repositoryService
          .put(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              const transformedResponse = this.transformResponse(response);
              this.response = new MatTableDataSource(transformedResponse);
              if (transformedResponse.length > 0) {
                this.columns = Object.keys(transformedResponse[0]);
              }
            },
            (error) => {
              console.error('Error:', error);
              if (error.error && typeof error.error === 'object') {
                this.errorMessage = 'Error: ' + JSON.stringify(error.error);
              } else {
                this.errorMessage = 'Error: ' + error.message;
              }
            }
          );
        break;
      case 'PATCH':
        this.repositoryService
          .patch(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              const transformedResponse = this.transformResponse(response);
              this.response = new MatTableDataSource(transformedResponse);
              if (transformedResponse.length > 0) {
                this.columns = Object.keys(transformedResponse[0]);
              }
            },
            (error) => {
              console.error('Error:', error);
              if (error.error && typeof error.error === 'object') {
                this.errorMessage = 'Error: ' + JSON.stringify(error.error);
              } else {
                this.errorMessage = 'Error: ' + error.message;
              }
            }
          );
        break;
      case 'DELETE':
        this.repositoryService
          .delete(this.endpoint, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              const transformedResponse = this.transformResponse(response);
              this.response = new MatTableDataSource(transformedResponse);
              if (transformedResponse.length > 0) {
                this.columns = Object.keys(transformedResponse[0]);
              }
            },
            (error) => {
              console.error('Error:', error);
              if (error.error && typeof error.error === 'object') {
                this.errorMessage = 'Error: ' + JSON.stringify(error.error);
              } else {
                this.errorMessage = 'Error: ' + error.message;
              }
            }
          );
        break;
      default:
        console.error('Invalid request type:', this.requestType);
    }
  }



  transformResponse(response: any): any[] {
    if (Array.isArray(response)) {
      return response;
    } else {
      return [response];
    }
  }


  generateRequestBody(): void {
    if (!this.endpoint || typeof this.endpoint !== 'string') {
      console.error('Error: Endpoint is not defined or is not a string');
      return;
    }
    const endpointParts = this.endpoint.split('/');
    const tableName = endpointParts[endpointParts.length - 1];

    this.repositoryService.getPropertiesByTableName(tableName).subscribe(
      (properties: Property[]) => {
        console.log(properties);
        const obj: { [key: string]: string | null } = {};
        properties.forEach((property) => {
          obj[property.name] = property.type === 'System.String' ? '' : null;
        });
        this.requestBody = JSON.stringify(obj, null, 2);
      },
      (error) => {
        console.error('Error:', error);
        if (error.error && typeof error.error === 'object') {
          this.errorMessage = 'Error: ' + JSON.stringify(error.error);
        } else {
          this.errorMessage = 'Error: ' + error.message;
        }
      }
    );
  }






}
