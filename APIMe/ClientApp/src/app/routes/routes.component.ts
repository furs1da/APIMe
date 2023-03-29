import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RouteDto } from '../../interfaces/response/routeDTO';
import { RouteTypeDto } from '../../interfaces/response/routeTypeDTO';
import { RepositoryService } from '../shared/services/repository.service';
import { AddEditRouteDialogComponent } from './add-edit-route-dialog/add-edit-route-dialog.component';
import { DeleteRouteDialogComponent } from './delete-route-dialog/delete-route-dialog.component';
import { TestRouteComponent } from './test-route/test-route.component';

@Component({
  selector: 'app-routes',
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.css']
})
export class RoutesComponent implements OnInit {
  routes: RouteDto[] = [];
  searchName = '';
  selectedRouteType = '';
  routeTypes: string[]= []; // You can populate this array with available route types
  filteredRoutes: RouteDto[]= [];

  constructor(private repositoryService: RepositoryService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadRoutes();
  }

  filterRoutes() {
    this.filteredRoutes = this.routes.filter(route => {
      const matchesName = route.name.toLowerCase().includes(this.searchName.toLowerCase());
      const matchesRouteType = this.selectedRouteType === '' || route.routeTypeName === this.selectedRouteType;
      return matchesName && matchesRouteType;
    });
  }


  // Add this method to the RoutesComponent class
  getUniqueRouteTypes() {
    const routeTypesSet = new Set(this.routes.map(route => route.routeTypeName));
    return Array.from(routeTypesSet);
  }


  loadRoutes(): void {
    this.repositoryService.getRoutes().subscribe((routes) => {
      this.routes = routes;
      this.filteredRoutes = this.routes;

      // Populate the routeTypes array with available route types
      this.routeTypes = this.getUniqueRouteTypes();

      // Apply initial filtering after loading the routes
      this.filterRoutes();
    });
  }

  openAddRouteDialog(): void {
    const dialogRef = this.dialog.open(AddEditRouteDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadRoutes();
      }
    });
  }

  openEditRouteDialog(route: RouteDto): void {
    const dialogRef = this.dialog.open(AddEditRouteDialogComponent, {
      width: '500px', // Set the max-width to 80% of the viewport width
      data: route,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadRoutes();
      }
    });
  }


  openTestRouteDialog(route: RouteDto): void {
    const dialogRef = this.dialog.open(TestRouteComponent, {
      width: '500px', // Set the max
      data: route,
    });

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
