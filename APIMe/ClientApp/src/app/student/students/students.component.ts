import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
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
  displayedColumns = ['firstName', 'lastName', 'actions'];
  filterForm: FormGroup;

  constructor(private repository: RepositoryService, public dialog: MatDialog, private fb: FormBuilder) {
    this.filterForm = this.fb.group({
      searchText: new FormControl(''),
      selectedSection: new FormControl(null),
    });
  }

  ngOnInit(): void {
    this.getStudents();
    this.getSections();
    this.filterForm.valueChanges.subscribe(() => {
      this.filterStudents();
    });
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
  }

  filterStudents(): void {
    const search = this.filterForm.get('searchText')?.value.toLowerCase() || '';
    const selectedSection = this.filterForm.get('selectedSection')?.value;

    this.filteredStudents = this.students.filter((student) => {
      const fullName = `${student.firstName} ${student.lastName}`.toLowerCase();
      const isInName = fullName.includes(search);
      const isInSection = selectedSection
        ? student.sections.some((section) => section.sectionName === selectedSection)
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
      data: { student: student },
      width: '500px',
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
