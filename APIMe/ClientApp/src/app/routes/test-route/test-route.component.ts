import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RepositoryService } from 'src/app/shared/services/repository.service';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { TestRouteResponse } from '../../../interfaces/response/TestRouteResponse';

@Component({
  selector: 'app-test-route',
  templateUrl: './test-route.component.html',
  styleUrls: ['./test-route.component.css']
})
export class TestRouteComponent implements OnInit {
  form: FormGroup;
  route: RouteDto;
  properties: any[] = [];
  response: TestRouteResponse | undefined;
  columns: string[] = [];
  showForm: boolean;
  isDeleteRoute: boolean;

  constructor(
    private fb: FormBuilder,
    private repositoryService: RepositoryService,
    public dialogRef: MatDialogRef<TestRouteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RouteDto
  ) {
    this.route = data;
    this.form = this.fb.group({});
    this.showForm = !(this.route.routeTypeName.includes('GET') || this.route.routeTypeName.includes('ERROR'));

    this.isDeleteRoute = this.route.routeTypeName.includes('DELETE');
  }

  ngOnInit(): void {
    this.getProperties(this.route.id);
  }

  getProperties(routeId: number): void {
    this.repositoryService.getPropertiesByRouteId(routeId).subscribe((properties) => {
      this.properties = properties;
      console.log(this.properties);
      this.createFormControls();
    });
  }

  createFormControls(): void {

    if (this.isDeleteRoute) {
      const idProperty = this.properties.find(p => p.name === 'id');
      if (idProperty) {
        this.form.addControl(idProperty.name, this.fb.control(''));
      }
    } else {
      this.properties.forEach((property) => {
        this.form.addControl(property.name, this.fb.control(''));
      });
    }
  }

  onSubmit() {
    if (this.form.valid) {
      const values = this.form.value;
      this.repositoryService.testRoute(this.route.id, values).subscribe(response => {
        this.response = response;
        if (response.records && response.records.length > 0) {
          this.columns = Object.keys(response.records[0]);
        }
      });
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
