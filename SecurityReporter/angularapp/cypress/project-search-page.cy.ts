import { ProjectSearchPageComponent } from '../src/app/project-search/component-pages/project-search-page/project-search-page.component'
import { mount } from 'cypress/angular';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule, MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('project-search-page.cy.ts', () => {
  it('playground', () => {
    cy.viewport(800, 800);
    mount(ProjectSearchPageComponent, {
      imports: [HttpClientModule, MatSnackBarModule, MatIconModule, MatFormFieldModule, MatInputModule, BrowserAnimationsModule],
      
    });
  })
})
