import { TestBed } from '@angular/core/testing';

import { GetEmailService } from './get-email.service';

describe('GetEmailService', () => {
  let service: GetEmailService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetEmailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
