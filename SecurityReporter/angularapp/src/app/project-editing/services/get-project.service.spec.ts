import { TestBed } from '@angular/core/testing';

import { GetProjectService } from './get-project.service';

describe('GetProjectService', () => {
  let service: GetProjectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetProjectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
