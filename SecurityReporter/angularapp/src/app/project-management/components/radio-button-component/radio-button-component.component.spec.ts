import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RadioButtonComponentComponent } from './radio-button-component.component';

describe('RadioButtonComponentComponent', () => {
  let component: RadioButtonComponentComponent;
  let fixture: ComponentFixture<RadioButtonComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RadioButtonComponentComponent]
    });
    fixture = TestBed.createComponent(RadioButtonComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
