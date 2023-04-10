import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteLogInfoDialogComponent } from './route-log-info-dialog.component';

describe('RouteLogInfoDialogComponent', () => {
  let component: RouteLogInfoDialogComponent;
  let fixture: ComponentFixture<RouteLogInfoDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RouteLogInfoDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RouteLogInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
