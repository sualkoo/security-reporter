import { TestBed } from '@angular/core/testing';

import { DeleteProjectsServiceService } from './delete-projects-service.service';

describe('DeleteProjectsServiceService', () => {
  let service: DeleteProjectsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteProjectsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
