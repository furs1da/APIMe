import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Section } from '../../../interfaces/request/section';
@Component({
  selector: 'app-add-edit-section',
  templateUrl: './add-edit-section.component.html',
  styleUrls: ['./add-edit-section.component.css']
})


export class AddEditSectionComponent implements OnInit {
  sectionForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AddEditSectionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { section: Section },
    private fb: FormBuilder,
  ) {
    this.sectionForm = this.fb.group({
      id: [data.section ? data.section.id : 0],
      sectionName: [data.section ? data.section.sectionName : '', Validators.required],
      professorName: [data.section ? data.section.professorName : '', Validators.required],
      accessCode: [data.section ? data.section.accessCode : '', Validators.required]
    });
  }

  ngOnInit(): void {
   
  }

  onSave(): void {
    if (this.sectionForm.valid) {
      // Add actual API call to save the section
      // this.http.post('/section/list', this.sectionForm.value).subscribe(() => this.dialogRef.close(true));
      this.dialogRef.close(true);
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}
