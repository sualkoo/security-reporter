import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectEditingPageComponent } from './project-editing-page.component';

describe('ProjectEditingPageComponent', () => {
  let component: ProjectEditingPageComponent;
  let fixture: ComponentFixture<ProjectEditingPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectEditingPageComponent]
    });
    fixture = TestBed.createComponent(ProjectEditingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
