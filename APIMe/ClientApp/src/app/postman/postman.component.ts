import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
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

  constructor(private repositoryService: RepositoryService, private dataSourcesService: DataSourcesService) { }

  ngOnInit(): void {
    this.dataSourcesService.getDataSources().subscribe(
      (data) => {
        this.tableNames = data.map((dataSource) => dataSource.name);
      },
      (error) => {
        console.error('Error fetching table names:', error);
      }
    );
  }

  isRequestBodyRequired(): boolean {
    return this.requestType === 'POST' || this.requestType === 'PUT' || this.requestType === 'PATCH';
  }

  suggestEndpoint(): void {
    if (this.tableNames.length > 0) {
      const randomTable = this.tableNames[Math.floor(Math.random() * this.tableNames.length)];
      this.endpoint = `https://example.com/api/${randomTable.toLowerCase()}`;
    } else {
      this.endpoint = 'https://example.com/suggested/endpoint';
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

    sendRequest(): void {
      const headers = new HttpHeaders(this.headers);
      switch(this.requestType) {
      case 'GET':
      this.repositoryService
        .get(this.endpoint, { headers })
        .subscribe(
          (response) => console.log('Response:', response),
          (error) => console.error('Error:', error)
        );
      break;
      case 'POST':
      case 'PUT':
      case 'PATCH':
      // Handle POST, PUT, PATCH requests
      break;
      case 'DELETE':
      // Handle DELETE requests
      break;
      default:
      console.error('Invalid request type:', this.requestType);
        }
      }

}
