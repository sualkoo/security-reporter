import { TestBed } from '@angular/core/testing';

import { GetProjectsServiceService } from './get-projects-service.service';

describe('GetProjectsServiceService', () => {
  let service: GetProjectsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetProjectsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
