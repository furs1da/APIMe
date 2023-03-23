import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionsComponent } from './sections/sections.component';
import { RouterModule } from '@angular/router';
import { AddEditSectionComponent } from './add-edit-section/add-edit-section.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';

@NgModule({
  declarations: [SectionsComponent, AddEditSectionComponent, ConfirmDialogComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatDividerModule,
    MatCardModule,
    RouterModule.forChild([
      { path: 'list', component: SectionsComponent },
      { path: 'add', component: AddEditSectionComponent },
      { path: 'edit/:id', component: AddEditSectionComponent }
    ])
  ]
})
export class SectionModule { }
