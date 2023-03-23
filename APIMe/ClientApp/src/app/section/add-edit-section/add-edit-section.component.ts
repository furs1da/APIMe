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
      @Inject(MAT_DIALOG_DATA) public data: { section?: Section },
      private fb: FormBuilder,
    ) {
      const section = data?.section;
      this.sectionForm = this.fb.group({
        id: [section ? section.id : 0],
        sectionName: [section ? section.sectionName : '', Validators.required], 
        accessCode: [section ? section.accessCode : '', Validators.required]
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
