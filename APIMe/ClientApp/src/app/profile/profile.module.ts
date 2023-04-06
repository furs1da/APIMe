
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTableModule } from '@angular/material/table';


import { StudentProfileComponent } from './student-profile/student-profile.component';
import { ProfessorProfileComponent } from './professor-profile/professor-profile.component';
import { AuthGuard } from '../shared/guards/auth.guard';
import { AdminGuard } from '../shared/guards/admin.guard';


@NgModule({
  declarations: [
    ProfessorProfileComponent,
    StudentProfileComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatTableModule,
    MatDividerModule,
    MatCardModule,
    MatSlideToggleModule,
    RouterModule.forChild([
      { path: 'professor', component: ProfessorProfileComponent, canActivate: [AuthGuard, AdminGuard] },
      { path: 'student', component: StudentProfileComponent, canActivate: [AuthGuard] }
    ])
  ]
})
export class ProfileModule { }
