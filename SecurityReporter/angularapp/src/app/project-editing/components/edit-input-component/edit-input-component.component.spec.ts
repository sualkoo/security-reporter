import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditInputComponentComponent } from './edit-input-component.component';

describe('EditInputComponentComponent', () => {
  let component: EditInputComponentComponent;
  let fixture: ComponentFixture<EditInputComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditInputComponentComponent]
    });
    fixture = TestBed.createComponent(EditInputComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
