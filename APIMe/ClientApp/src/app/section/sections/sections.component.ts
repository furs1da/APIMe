import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Section } from '../../../interfaces/request/section';
import { AddEditSectionComponent } from '../add-edit-section/add-edit-section.component';


@Component({
  selector: 'app-sections',
  templateUrl: './sections.component.html',
  styleUrls: ['./sections.component.css']
})
export class SectionsComponent implements OnInit {

  sections: Section[] = [];

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getSections();
  }

  getSections(): void {
    // Replace with actual API call
    // this.http.get<Section[]>('/section/list').subscribe((sections) => (this.sections = sections));

    this.sections = [
      {
        id: 1,
        sectionName: 'Section 1',
        professorName: 'Prof. John Doe',
        accessCode: 'ABC123'
      },
      {
        id: 2,
        sectionName: 'Section 2',
        professorName: 'Prof. Jane Smith',
        accessCode: 'XYZ789'
      }
    ];
  }

  addSection(): void {
    const dialogRef = this.dialog.open(AddEditSectionComponent, {
      width: '500px'
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
    // Add actual API call to delete the section
    // this.http.delete(`/section/list/${section.id}`).subscribe(() => this.getSections());
  }
}
