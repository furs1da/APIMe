import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { RouteDto } from '../../../interfaces/response/routeDTO';
import { RepositoryService } from '../../shared/services/repository.service';


@Component({
  selector: 'app-delete-route-dialog',
  templateUrl: './delete-route-dialog.component.html',
  styleUrls: ['./delete-route-dialog.component.css']
})
export class DeleteRouteDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteRouteDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RouteDto,
    private repositoryService: RepositoryService
  ) { }

  deleteRoute(): void {
    this.repositoryService.deleteRoute(this.data.id).subscribe(() => {
      this.dialogRef.close(true);
    });
  }
}
