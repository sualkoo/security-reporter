import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteDataGridComponentComponent } from './delete-data-grid-component.component';

describe('DeleteDataGridComponentComponent', () => {
  let component: DeleteDataGridComponentComponent;
  let fixture: ComponentFixture<DeleteDataGridComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeleteDataGridComponentComponent]
    });
    fixture = TestBed.createComponent(DeleteDataGridComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
