import { Component, OnInit } from '@angular/core';
import { RepositoryService } from '../../shared/services/repository.service';
import { ProfessorProfile } from 'src/interfaces/profile/profile/professorProfile';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../../shared/services/authentication.service';
import { AuthResponseDto } from '../../../interfaces/response/authResponseDto';



@Component({
  selector: 'app-professor-profile',
  templateUrl: './professor-profile.component.html',
  styleUrls: ['./professor-profile.component.css']
})
export class ProfessorProfileComponent implements OnInit {

  professor: ProfessorProfile | undefined
  errorMessage: string = "";
  professorProfileForm: FormGroup;

  constructor(
    private repositoryService: RepositoryService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private authService: AuthenticationService
  ) {
    this.professorProfileForm = this.fb.group({
      id: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  ngOnInit(): void {
    this.getProfessorProfile();
  }

  getProfessorProfile(): void {
    this.repositoryService.getProfessorProfile("profileApi/profile").subscribe(
      (professor) => {
        console.log(professor);
        this.professor = professor;
        this.professorProfileForm.patchValue({
          id: this.professor.id,
          firstName: this.professor.firstName,
          lastName: this.professor.lastName,
          email: this.professor.email,
        });
      },
      (error) => {
        this.errorMessage = error;
      }
    );
  }


  onSave = (professorFormValue: any) => {
    const professorProfileDTO: ProfessorProfile = {
      id: professorFormValue.id,
      email: professorFormValue.email,
      firstName: professorFormValue.firstName,
      lastName: professorFormValue.lastName,
    };

    console.log(professorProfileDTO);

    this.repositoryService.updateProfessorProfile(professorProfileDTO).subscribe({
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
        this.professorProfileForm.markAsPristine();
      },
      error: (errorResponse: HttpErrorResponse) => {
        if (errorResponse.error) {
          console.log(errorResponse.error);
          this.errorMessage = errorResponse.error.message;
          this.snackBar.open(this.errorMessage, 'Close', {
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
