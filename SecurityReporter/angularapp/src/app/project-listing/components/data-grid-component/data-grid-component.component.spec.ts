import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataGridComponentComponent } from './data-grid-component.component';

describe('DataGridComponentComponent', () => {
  let component: DataGridComponentComponent;
  let fixture: ComponentFixture<DataGridComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DataGridComponentComponent]
    });
    fixture = TestBed.createComponent(DataGridComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
