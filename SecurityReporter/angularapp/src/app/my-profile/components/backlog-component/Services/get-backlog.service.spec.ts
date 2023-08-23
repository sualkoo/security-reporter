import { TestBed } from '@angular/core/testing';

import { GetBacklogService } from './get-backlog.service';

describe('GetBacklogService', () => {
  let service: GetBacklogService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetBacklogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
