import { TestBed } from '@angular/core/testing';

import { ProjectDataServiceService } from './project-data-service.service';

describe('ProjectDataServiceService', () => {
  let service: ProjectDataServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProjectDataServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
