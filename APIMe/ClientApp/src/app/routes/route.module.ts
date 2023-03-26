import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';

import { ReactiveFormsModule } from '@angular/forms';

import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { AddEditRouteDialogComponent } from './add-edit-route-dialog/add-edit-route-dialog.component';
import { DeleteRouteDialogComponent } from './delete-route-dialog/delete-route-dialog.component';
import { RouteCardComponent } from './route-card/route-card.component';
import { RoutesComponent } from './routes.component';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';


@NgModule({
  declarations: [RouteCardComponent, AddEditRouteDialogComponent, DeleteRouteDialogComponent, RoutesComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatDividerModule,
    MatCardModule,
    MatSlideToggleModule,
    RouterModule.forChild([
      { path: 'list', component: RoutesComponent },
    ])
  ]
})
export class RouteModule { }