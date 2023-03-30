import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Section } from '../../../interfaces/request/section';
import { Student } from '../../../interfaces/request/student';
import { RepositoryService } from '../../shared/services/repository.service';
import { DeleteDialogComponent } from '../delete-dialog/delete-dialog.component';
import { InfoDialogComponent } from '../info-dialog/info-dialog.component';

@Component({
  selector: 'app-student-list',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css'],
})
export class StudentListComponent implements OnInit {
  students: Student[] = [];
  sections: Section[] = [];
  filteredStudents: Student[] = [];
  searchText = '';
  selectedSection: string | null = null;
  displayedColumns = ['firstName', 'lastName', 'actions'];

  constructor(private repository: RepositoryService, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getStudents();
    this.getSections();
  }

  getStudents(): void {
    this.repository.getStudents().subscribe((students) => {
      this.students = students;
      this.filteredStudents = students;
    });
  }

  getSections(): void {
    this.repository.getSectionsStudent().subscribe((sections) => {
      this.sections = sections;
    });

    console.log(this.sections);
  }

  filterStudents(): void {
    const search = this.searchText.toLowerCase();
    this.filteredStudents = this.students.filter((student) => {
      const fullName = `${student.firstName} ${student.lastName}`.toLowerCase();
      const isInName = fullName.includes(search);
      const isInSection = this.selectedSection
        ? student.sections.some((section) => section.sectionName === this.selectedSection)
        : true;
      return isInName && isInSection;
    });
  }

  openInfoDialog(student: Student): void {
    this.dialog.open(InfoDialogComponent, {
      data: { student: student },
      width: '500px',
    });
  }

  openDeleteDialog(student: Student): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {
        title: 'Delete Student',
        message: `Are you sure you want to delete ${student.firstName} ${student.lastName}?`,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.repository.deleteStudent(student.id).subscribe(() => {
          this.getStudents();
        });
      }
    });
  }
}
