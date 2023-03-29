import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditRouteDialogComponent } from './add-edit-route-dialog.component';

describe('AddEditRouteDialogComponent', () => {
  let component: AddEditRouteDialogComponent;
  let fixture: ComponentFixture<AddEditRouteDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddEditRouteDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddEditRouteDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
