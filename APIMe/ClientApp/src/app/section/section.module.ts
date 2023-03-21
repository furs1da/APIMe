import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionsComponent } from './sections/sections.component';
import { RouterModule } from '@angular/router';
import { AddEditSectionComponent } from './add-edit-section/add-edit-section.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [SectionsComponent, AddEditSectionComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([
      { path: 'list', component: SectionsComponent },
      { path: 'section', component: AddEditSectionComponent }
    ])
  ]
})
export class SectionModule { }
