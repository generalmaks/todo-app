import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeftSidebarButtonComponent } from './left-sidebar-button.component';

describe('LeftSidebarButtonComponent', () => {
  let component: LeftSidebarButtonComponent;
  let fixture: ComponentFixture<LeftSidebarButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeftSidebarButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeftSidebarButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
