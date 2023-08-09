import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AfterLoginPageComponent } from './after-login-page.component';

describe('AfterLoginPageComponent', () => {
  let component: AfterLoginPageComponent;
  let fixture: ComponentFixture<AfterLoginPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AfterLoginPageComponent]
    });
    fixture = TestBed.createComponent(AfterLoginPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
