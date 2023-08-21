import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileDragDropComponent } from './file-drag-drop.component';

describe('FileDragDropComponent', () => {
  let component: FileDragDropComponent;
  let fixture: ComponentFixture<FileDragDropComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FileDragDropComponent]
    });
    fixture = TestBed.createComponent(FileDragDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
