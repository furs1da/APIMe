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

  constructor(
    private fb: FormBuilder,
    private repositoryService: RepositoryService,
    public dialogRef: MatDialogRef<TestRouteComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RouteDto
  ) {
    this.route = data;
    this.form = this.fb.group({});
  }

  ngOnInit(): void {
    this.getProperties(this.route.routeTypeId);
  }

  getProperties(routeTypeId: number): void {
    this.repositoryService.getPropertiesByRouteTypeId(routeTypeId).subscribe((properties) => {
      this.properties = properties;
      this.createFormControls();
    });
  }

  createFormControls(): void {
    this.properties.forEach((property) => {
      this.form.addControl(property.name, this.fb.control(''));
    });
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