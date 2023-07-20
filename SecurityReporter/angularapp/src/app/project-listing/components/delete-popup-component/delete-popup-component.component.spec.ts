import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DeletePopupComponentComponent } from './delete-popup-component.component';

describe('DeletePopupComponentComponent', () => {
  let component: DeletePopupComponentComponent;
  let fixture: ComponentFixture<DeletePopupComponentComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeletePopupComponentComponent]
    });
    fixture = TestBed.createComponent(DeletePopupComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});


