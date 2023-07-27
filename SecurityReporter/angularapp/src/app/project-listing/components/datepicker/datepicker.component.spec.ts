import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FiltersDatepickerComponent } from './datepicker.component';

describe('DatepickerComponent', () => {
  let component: FiltersDatepickerComponent;
  let fixture: ComponentFixture<FiltersDatepickerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FiltersDatepickerComponent]
    });
    fixture = TestBed.createComponent(FiltersDatepickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
