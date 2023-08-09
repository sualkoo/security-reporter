import { ProjectSearchPageComponent } from '../src/app/project-search/component-pages/project-search-page/project-search-page.component'
import { mount } from 'cypress/angular';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule, MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

describe('project-search-page.cy.ts', () => {
  it('playground', () => {
    cy.viewport(800, 800);
    mount(ProjectSearchPageComponent, {
      imports: [HttpClientModule, MatSnackBarModule, MatIconModule],
      providers: [
        { provide: MAT_SNACK_BAR_DATA, useValue: {} },
        { provide: MatSnackBarRef, useValue: {} },
      ]
    });
  })
})
