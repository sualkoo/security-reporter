import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BacklogComponentComponent } from './backlog-component.component';

describe('BacklogComponentComponent', () => {
  let component: BacklogComponentComponent;
  let fixture: ComponentFixture<BacklogComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BacklogComponentComponent]
    });
    fixture = TestBed.createComponent(BacklogComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
