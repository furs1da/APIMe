import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionsComponent } from './sections/sections.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [SectionsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: SectionsComponent }
    ])
  ]
})
export class SectionModule { }
