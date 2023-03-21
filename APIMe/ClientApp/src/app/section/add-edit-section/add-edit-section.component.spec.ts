import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditSectionComponent } from './add-edit-section.component';

describe('AddEditSectionComponent', () => {
  let component: AddEditSectionComponent;
  let fixture: ComponentFixture<AddEditSectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddEditSectionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
