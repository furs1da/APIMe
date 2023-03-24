import { Component, OnInit } from '@angular/core';
import { Section } from '../../../interfaces/request/section';
import { SectionClass } from '../../../classes/SectionClass';
import { AddEditSectionComponent } from '../add-edit-section/add-edit-section.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { RepositoryService } from '../../shared/services/repository.service';

@Component({
  selector: 'app-sections',
  templateUrl: './sections.component.html',
  styleUrls: ['./sections.component.css']
})
export class SectionsComponent implements OnInit {

  sections: Section[] = [];

  constructor(private repositoryService: RepositoryService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getSections();
  }

  getSections(): void {
    this.repositoryService.getSections("sectionApi/sections").subscribe(
      (sections) => {
        this.sections = sections;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  addSection(): void {
    const newSection = new SectionClass({
      id: 0,
      sectionName: '',
      professorName: '',
      accessCode: ''
      });

    const dialogRef = this.dialog.open(AddEditSectionComponent, {
      width: '500px',
      data: { newSection }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.getSections();
      }
    });
  }


  editSection(section: Section): void {
    const dialogRef = this.dialog.open(AddEditSectionComponent, {
      width: '500px',
      data: { section }
    });


    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.getSections();
      }
    });
  }

  deleteSection(section: Section): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Delete Section',
        message: `Are you sure you want to delete "${section.sectionName}" section?`
      }
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.repositoryService.deleteSection(section.id).subscribe(() => {
          this.getSections();
        });
      }
    });
  }
}
