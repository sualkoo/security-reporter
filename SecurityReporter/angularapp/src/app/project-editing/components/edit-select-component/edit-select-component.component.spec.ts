import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditSelectComponentComponent } from './edit-select-component.component';

describe('EditSelectComponentComponent', () => {
  let component: EditSelectComponentComponent;
  let fixture: ComponentFixture<EditSelectComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EditSelectComponentComponent]
    });
    fixture = TestBed.createComponent(EditSelectComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
