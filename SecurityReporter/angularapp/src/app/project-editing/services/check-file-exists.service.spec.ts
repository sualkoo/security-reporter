import { TestBed } from '@angular/core/testing';

import { CheckFileExistsService } from './check-file-exists.service';

describe('CheckFileExistsService', () => {
  let service: CheckFileExistsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CheckFileExistsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
