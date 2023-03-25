import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RepositoryService } from '../../shared/services/repository.service';
import { RouteTypesService } from '../../shared/services/route-types.service';
import { DataSourcesService } from '../../shared/services/data-sources.service';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { DataSourceDto } from '../../../interfaces/response/dataSourceDTO';
import { RouteTypeDto } from '../../../interfaces/response/routeTypeDTO';


@Component({
  selector: 'app-add-edit-route-dialog',
  templateUrl: './add-edit-route-dialog.component.html',
  styleUrls: ['./add-edit-route-dialog.component.css']
})
export class AddEditRouteDialogComponent implements OnInit {
  routeForm: FormGroup;
  routeTypes: RouteTypeDto[] = [];
  dataSources: DataSourceDto[] = [];

  constructor(
    public dialogRef: MatDialogRef<AddEditRouteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RouteDto | null,
    private fb: FormBuilder,
    private repositoryService: RepositoryService,
    private routeTypesService: RouteTypesService,
    private dataSourcesService: DataSourcesService
  ) {
    this.routeForm = this.fb.group({
      name: [data ? data.name : '', Validators.required],
      description: [data ? data.description : '', Validators.required],
      routeTypeId: [data ? data.routeTypeId : '', Validators.required
      ],
      dataTableName: [data ? data.dataTableName : '', Validators.required],
      isVisible: [data ? data.isVisible : false]
    });
  }

  ngOnInit(): void {
    this.loadRouteTypes();
    this.loadDataSources();
  }

  loadRouteTypes(): void {
    this.routeTypesService.getRouteTypes().subscribe((routeTypes) => {
      this.routeTypes = routeTypes;
    });
  }

  loadDataSources(): void {
    this.dataSourcesService.getDataSources().subscribe((dataSources) => {
      this.dataSources = dataSources;
    });
  }

  submitForm(): void {
    if (this.routeForm.valid) {
      if (this.data) {
        const updatedRoute: RouteDto = { ...this.data, ...this.routeForm.value };
        this.repositoryService.updateRoute(updatedRoute).subscribe(() => {
          this.dialogRef.close(true);
        });
      } else {
        const newRoute: RouteDto = this.routeForm.value;
        this.repositoryService.createRoute(newRoute).subscribe(() => {
          this.dialogRef.close(true);
        });
      }
    }
  }
}
