import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RouteLogComponent } from './route-log.component';

describe('RouteLogComponent', () => {
  let component: RouteLogComponent;
  let fixture: ComponentFixture<RouteLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RouteLogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RouteLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
