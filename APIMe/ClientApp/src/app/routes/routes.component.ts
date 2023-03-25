import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RouteDto } from '../../interfaces/response/routeDTO';
import { RepositoryService } from '../shared/services/repository.service';
import { AddEditRouteDialogComponent } from './add-edit-route-dialog/add-edit-route-dialog.component';
import { DeleteRouteDialogComponent } from './delete-route-dialog/delete-route-dialog.component';


@Component({
  selector: 'app-routes',
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.css']
})
export class RoutesComponent implements OnInit {
  routes: RouteDto[] = [];

  constructor(private repositoryService: RepositoryService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadRoutes();
  }

  loadRoutes(): void {
    this.repositoryService.getRoutes().subscribe((routes) => {
      this.routes = routes;
    });
  }

  openAddRouteDialog(): void {
    const dialogRef = this.dialog.open(AddEditRouteDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadRoutes();
      }
    });
  }

  openEditRouteDialog(route: RouteDto): void {
    const dialogRef = this.dialog.open(AddEditRouteDialogComponent, { data: route });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadRoutes();
      }
    });
  }

  openDeleteRouteDialog(route: RouteDto): void {
    const dialogRef = this.dialog.open(DeleteRouteDialogComponent, { data: route });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadRoutes();
      }
    });
  }
}
