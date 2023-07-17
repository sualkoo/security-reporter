import { TestBed } from '@angular/core/testing';

import { GetProjectsCountService } from './get-projects-count.service';

describe('GetProjectsCountService', () => {
  let service: GetProjectsCountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetProjectsCountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
