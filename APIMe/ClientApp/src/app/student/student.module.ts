import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatOptionModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select'; // Add this import

import { InfoDialogComponent } from './info-dialog/info-dialog.component';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { StudentListComponent } from './students/students.component';

@NgModule({
  declarations: [StudentListComponent, InfoDialogComponent, DeleteDialogComponent],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatDividerModule,
    MatCardModule,
    MatOptionModule,
    MatTableModule,
    MatSelectModule, // Add MatSelectModule here
    RouterModule.forChild([
      { path: 'list', component: StudentListComponent }
    ])
  ]
})
export class StudentModule { }
