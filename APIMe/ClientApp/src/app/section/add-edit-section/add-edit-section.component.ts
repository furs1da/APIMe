import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Section } from '../../../interfaces/request/section';
import { RepositoryService } from '../../shared/services/repository.service';

@Component({
  selector: 'app-add-edit-section',
  templateUrl: './add-edit-section.component.html',
  styleUrls: ['./add-edit-section.component.css']
})


export class AddEditSectionComponent implements OnInit {
  sectionForm: FormGroup;
  errorMessage: string = "";

  constructor(
    public dialogRef: MatDialogRef<AddEditSectionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { section?: Section },
    private fb: FormBuilder, private repositoryService: RepositoryService,
  ) {
    const section = data?.section;
    this.sectionForm = this.fb.group({
      id: [section ? section.id : 0],
      sectionName: [section ? section.sectionName : '', Validators.required],
      accessCode: [section ? section.accessCode : '', Validators.required],
    });

  }

  ngOnInit(): void { }

  onSave = (sectionFormValue: any) => {
    if (this.sectionForm.valid) {
      const sectionVal = { ...sectionFormValue };
      if (this.sectionForm.controls.id.value) {
        // Edit Section
        const sectionDTO: Section = {
          id: sectionVal.id,
          accessCode: sectionVal.accessCode,
          sectionName: sectionVal.sectionName,
          professorName: ''
        };

        this.repositoryService.updateSection(sectionDTO).subscribe({
          next: (_) =>
            this.dialogRef.close(true),
          error: (err: string) => {
            if (err) {
              this.errorMessage = err;
            } else {
              this.errorMessage = 'An unexpected error occurred. Please try again later.';
            }
          }
        });
      }
      else {
        const sectionDTO: Section = {
          id: 0,
          accessCode: sectionVal.accessCode,
          sectionName: sectionVal.sectionName,
          professorName: ''
        };


          // Add actual API call to save the section
        this.repositoryService.createSection(sectionDTO).subscribe({
            next: (_) =>
              this.dialogRef.close(true),
          error: (err: string) => {    
            if (err) {
              this.errorMessage = err;
            } else {
              this.errorMessage = 'An unexpected error occurred. Please try again later.';
            }
          }
          });
        
      }
    }
  }


  onCancel(): void {
    this.dialogRef.close(false);
  }
}
