import { TestBed } from '@angular/core/testing';

import { DeleteProjectServiceService } from './delete-project-service.service';

describe('DeleteProjectServiceService', () => {
  let service: DeleteProjectServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DeleteProjectServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
