import { Component, OnInit } from '@angular/core';
import { RepositoryService } from '../../shared/services/repository.service';
import { StudentProfile } from 'src/interfaces/profile/profile/studentProfile';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../shared/services/authentication.service';
import { AuthResponseDto } from '../../../interfaces/response/authResponseDto';

@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.css']
})
export class StudentProfileComponent implements OnInit {

  student: StudentProfile | undefined
  errorMessage: string = "";
  studentProfileForm: FormGroup;

  constructor(
    private repositoryService: RepositoryService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private authService: AuthenticationService
  ) {
    this.studentProfileForm = this.fb.group({
      id: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      studentId: ['', [Validators.required, Validators.pattern(/^\d{7}$/)]],
    });
  }

  ngOnInit(): void {
    this.getStudentProfile();
  }

  getStudentProfile(): void {
    this.repositoryService.getStudentProfile("profileApi/studentProfile").subscribe(
      (student) => {
        console.log(student);
        this.student = student;
        this.studentProfileForm.patchValue({
          id: this.student.id,
          firstName: this.student.firstName,
          lastName: this.student.lastName,
          email: this.student.email,
          studentId: this.student.studentId,
        });
      },
      (error) => {
        this.errorMessage = error.message;
      }
    );
  }

  onSave = (studentFormValue: any) => {
    const studentProfileDTO: StudentProfile = {
      id: studentFormValue.id,
      email: studentFormValue.email,
      firstName: studentFormValue.firstName,
      lastName: studentFormValue.lastName,
      studentId: studentFormValue.studentId,
    };
    console.log(studentProfileDTO);

    this.repositoryService.updateStudentProfile(studentProfileDTO).subscribe({
      next: (res: AuthResponseDto) => {
        if (res.token) {
          localStorage.setItem("token", res.token);
          this.authService.sendAuthStateChangeNotification(true);
        }

        this.snackBar.open('Profile updated successfully', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: 'success-snackbar'
        });
        this.studentProfileForm.markAsPristine();
      },
      error: (errorResponse: HttpErrorResponse) => {
        if (errorResponse.error) {
          console.log(errorResponse.error);
          this.errorMessage = errorResponse.error.message;
          this.snackBar.open('An error occured while updating your profile', 'Close', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'bottom',
            panelClass: 'error-snackbar'
          });
        } else {
          this.errorMessage = 'An unexpected error occurred. Please try again later.';
        }
      }
    });
  }
}
