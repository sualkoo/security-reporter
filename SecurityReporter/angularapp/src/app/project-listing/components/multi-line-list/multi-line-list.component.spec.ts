import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiLineListComponent } from './multi-line-list.component';

describe('MultiLineListComponent', () => {
  let component: MultiLineListComponent;
  let fixture: ComponentFixture<MultiLineListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MultiLineListComponent]
    });
    fixture = TestBed.createComponent(MultiLineListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
