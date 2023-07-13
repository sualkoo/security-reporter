import { TestBed } from '@angular/core/testing';

import { ProjectDataService } from './project-data-service';

describe('ProjectDataServiceService', () => {
  let service: ProjectDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
