import { Component, OnInit } from '@angular/core';
import { RepositoryService } from '../../shared/services/repository.service';
import { ProfessorProfile } from 'src/interfaces/profile/profile/professorProfile';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

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
    private fb: FormBuilder
  ) {
    this.professorProfileForm = this.fb.group({
      id: [this.professor ? this.professor.id : 0],
      firstName: [this.professor ? this.professor.firstName : '', Validators.required],
      lastName: [this.professor ? this.professor.lastName : '', Validators.required],
      email: [this.professor ? this.professor.email : '', Validators.required]    
    });
  }

 
  ngOnInit(): void {
    this.getProfessorProfile();
  }

  getProfessorProfile(): void {
    this.repositoryService.getProfessorProfile("profileApi/profile").subscribe(
      (professor) => {
        console.log(professor)
        this.professor = professor;
      },
      (error) => {
        console.error(error);
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

    console.log(professorProfileDTO)

    this.repositoryService.updateProfessorProfile(professorProfileDTO).subscribe({
      next: (_) =>{
          error: (err: string) => {
            if (err) {
              this.errorMessage = err;
            } else {
              this.errorMessage = 'An unexpected error occurred. Please try again later.';
            }
          }
      }
    });

  }
}
