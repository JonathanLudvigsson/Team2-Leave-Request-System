import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfigLeaveTypesComponent } from './config-leave-types.component';

describe('ConfigLeaveTypesComponent', () => {
  let component: ConfigLeaveTypesComponent;
  let fixture: ComponentFixture<ConfigLeaveTypesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConfigLeaveTypesComponent]
    });
    fixture = TestBed.createComponent(ConfigLeaveTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
