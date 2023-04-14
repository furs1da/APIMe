import { Component, Inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RepositoryService } from 'src/app/shared/services/repository.service';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { TestRouteResponse } from '../../../interfaces/response/testRouteResponse';

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
    @Inject(MAT_DIALOG_DATA) public data: RouteDto,
    private changeDetectorRef: ChangeDetectorRef
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
      this.createFormControls();
    });
  }

  createFormControls(): void {
    if (this.isDeleteRoute) {
      const idProperty = this.properties.find(p => p.name === 'Id');
      if (idProperty) {
        this.form.addControl('Id', this.fb.control('', Validators.required));

        // Subscribe to value changes of the 'Id' form control
        this.form.get('Id')?.valueChanges.subscribe(value => {
        });

        // Trigger change detection
        this.changeDetectorRef.detectChanges();
      }
    } else {
      this.properties.forEach((property) => {
        this.form.addControl(property.name, this.fb.control(''));
      });
    }
    this.form.valueChanges.subscribe(value => {
    });
  }


  onIdInputChange(event: any): void {
    this.form.get('Id')?.setValue(event.target.value); 
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
