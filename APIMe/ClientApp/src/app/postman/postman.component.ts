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
  public headers: { key: string; value: string }[] = [];
  tableNames: string[] = [];
  baseUrl: string = "";
  errorMessage: string | null = null;
  successMessage: string | null = null;

  response: MatTableDataSource<any> | undefined;
  columns: string[] = [];




  constructor(private repositoryService: RepositoryService, private dataSourcesService: DataSourcesService, @Inject('BASE_URL') baseUrl: string) {
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


  //ngAfterViewInit(): void {
  //  const endpointInput = document.querySelector('input[matInput]');
  //  if (endpointInput) {
  //    fromEvent(endpointInput, 'input')
  //      .pipe(debounceTime(1000))
  //      .subscribe(() => {
  //        this.generateRequestBody();
  //      });
  //  }
  //}


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
    if (this.headers.length === 0) {
      // Set default header key and value for the first header
      this.headers.push({ key: 'Content-Type', value: 'application/json' });
    } else {
      // Generate a new unique header key for subsequent headers
      let newKey = `Header-${this.headers.length + 1}`;
      while (this.headers.find((header) => header.key === newKey)) {
        newKey = `Header-${parseInt(newKey.split('-')[1]) + 1}`;
      }
      this.headers.push({ key: newKey, value: '' });
    }
  }



  removeHeader(key: string): void {
    this.headers = this.headers.filter((header) => header.key !== key);
  }

    headerKeys(): string[] {
      return Object.keys(this.headers);
    }

  // Modify the sendRequest method in the PostmanComponent class:

  sendRequest(): void {
    this.errorMessage = null;
    this.successMessage = null;

    if (this.isRequestBodyRequired() && !this.isValidJSON(this.requestBody)) {
      this.errorMessage = 'Error: The request body is not a valid JSON object.';
      return;
    }


    let headers = new HttpHeaders();
    for (const header of this.headers) {
      headers = headers.append(header.key, header.value);
    }


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
      case 'DELETE':
        this.repositoryService
          .delete(this.endpoint, { headers })
          .subscribe(
            (response) => {
              console.log('Response:', response);
              if (response === null) {
                this.successMessage = 'Record was successfully deleted.';
              } else {
                const transformedResponse = this.transformResponse(response);
                this.response = new MatTableDataSource(transformedResponse);
                if (transformedResponse.length > 0) {
                  this.columns = Object.keys(transformedResponse[0]);
                }
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

  isValidJSON(jsonString: string): boolean {
    try {
      JSON.parse(jsonString);
      return true;
    } catch (e) {
      return false;
    }
  }

  generateRequestBody(): void {
    if (!this.endpoint || typeof this.endpoint !== 'string') {
      console.error('Error: Endpoint is not defined or is not a string');
      return;
    }
    const endpointParts = this.endpoint.split('/');

    let tableName: string;

    if (this.requestType === 'PATCH') {
      tableName = endpointParts[endpointParts.length - 2];
    } else {
      tableName = endpointParts[endpointParts.length - 1];
    }

    this.repositoryService.getPropertiesByTableName(tableName).subscribe(
      (properties: Property[]) => {
        console.log(properties);
        const keyValuePairs = properties
          .filter((property) => {
            // Exclude the "Id" field for PATCH requests
            if (this.requestType === 'PATCH' && property.name === 'Id') {
              return false;
            }
            return true;
          })
          .map((property) => {
            let value = "";
            if (property.type !== 'Int32' && property.type !== 'Decimal' && property.type !== 'Double') {
              value = '""';
            }
            return `"${property.name}": ${value}`;
          });

        this.requestBody = `{\n  ${keyValuePairs.join(",\n  ")}\n}`;
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
