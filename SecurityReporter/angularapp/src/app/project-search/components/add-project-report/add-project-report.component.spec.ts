import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddProjectReportComponent } from './add-project-report.component';

describe('AddProjectReportComponent', () => {
  let component: AddProjectReportComponent;
  let fixture: ComponentFixture<AddProjectReportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AddProjectReportComponent]
    });
    fixture = TestBed.createComponent(AddProjectReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
