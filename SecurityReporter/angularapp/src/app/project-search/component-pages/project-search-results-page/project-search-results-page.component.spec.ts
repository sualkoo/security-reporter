import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSearchResultsPageComponent } from './project-search-results-page.component';

describe('ProjectSearchResultsPageComponent', () => {
  let component: ProjectSearchResultsPageComponent;
  let fixture: ComponentFixture<ProjectSearchResultsPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProjectSearchResultsPageComponent]
    });
    fixture = TestBed.createComponent(ProjectSearchResultsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
