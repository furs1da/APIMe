import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfessorProfileComponent } from './professor-profile.component';

describe('ProfessorProfileComponent', () => {
  let component: ProfessorProfileComponent;
  let fixture: ComponentFixture<ProfessorProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProfessorProfileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfessorProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
