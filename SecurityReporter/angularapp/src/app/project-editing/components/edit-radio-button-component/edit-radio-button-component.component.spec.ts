import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditRadioButtonComponentComponent } from './edit-radio-button-component.component';

describe('EditRadioButtonComponentComponent', () => {
  let component: EditRadioButtonComponentComponent;
  let fixture: ComponentFixture<EditRadioButtonComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditRadioButtonComponentComponent]
    });
    fixture = TestBed.createComponent(EditRadioButtonComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
