import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSearchPageComponent } from './project-search-page.component';

describe('ProjectSearchComponent', () => {
  let component: ProjectSearchPageComponent;
  let fixture: ComponentFixture<ProjectSearchPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectSearchPageComponent],
    });
    fixture = TestBed.createComponent(ProjectSearchPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
