import { HttpHeaders } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { Property } from '../../interfaces/response/property';
import { DataSourcesService } from '../shared/services/data-sources.service';
import { RepositoryService } from '../shared/services/repository.service';




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

  response: Property[][] | undefined;
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
          this.errorMessage = 'Error: ' + error.message;
        }
    );
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
    } else {
      this.endpoint = `${this.baseUrl}routeApi/records/orders`;
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
              this.response = response;
              if (response && typeof response === 'object') {
                this.columns = Object.keys(response);
              }
            },
            (error) => {
              console.error('Error:', error);
              this.errorMessage = 'Error: ' + error.message;
            }
          );
        break;
      case 'POST':
        this.repositoryService
          .post(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              this.response = response;
              if (response && typeof response === 'object') {
                this.columns = Object.keys(response);
              }
            },
            (error) => {
              console.error('Error:', error);
              this.errorMessage = 'Error: ' + error.message;
            }
          );
        break;
      case 'PUT':
        this.repositoryService
          .put(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              this.response = response;
              if (response && typeof response === 'object') {
                this.columns = Object.keys(response);
              }
            },
            (error) => {
              console.error('Error:', error);
              this.errorMessage = 'Error: ' + error.message;
            }
          );
        break;
      case 'PATCH':
        this.repositoryService
          .patch(this.endpoint, this.requestBody, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              this.response = response;
              if (response && typeof response === 'object') {
                this.columns = Object.keys(response);
              }
            },
            (error) => {
              console.error('Error:', error);
              this.errorMessage = 'Error: ' + error.message;
            }
          );
        break;
      case 'DELETE':
        this.repositoryService
          .delete(this.endpoint, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              this.response = response;
              if (response && typeof response === 'object') {
                this.columns = Object.keys(response);
              }
            },
            (error) => {
              console.error('Error:', error);
              this.errorMessage = 'Error: ' + error.message;
            }
          );
        break;
      default:
        console.error('Invalid request type:', this.requestType);
    }
  }



  transformResponse(response: any): any[] {
    if (Array.isArray(response)) {
      return response.map((item) => {
        const obj: { [key: string]: any } = {};
        for (const key in item) {
          if (item.hasOwnProperty(key)) {
            obj[key] = item[key];
          }
        }
        return obj;
      });
    } else {
      const obj: { [key: string]: any } = {};
      for (const key in response) {
        if (response.hasOwnProperty(key)) {
          obj[key] = response[key];
        }
      }
      return [obj];
    }
  }


}
