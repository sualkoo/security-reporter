import { CustomSnackbarComponent } from '../src/app/project-search/components/custom-snackbar/custom-snackbar.component'
import { MatSnackBarModule, MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { mount } from 'cypress/angular';

describe('custom-snackbar.cy.ts', () => {
  it('should display the snackbar', () => {
    const messageData = {
      messageType: 'info',
      message: 'This is an info message',
    };

    // Mount the component
    mount(CustomSnackbarComponent, {
      imports: [MatIconModule, MatSnackBarModule],
      providers: [
        { provide: MAT_SNACK_BAR_DATA, useValue: messageData },
        { provide: MatSnackBarRef, useValue: {} },
      ]
    });

    // Verify the icon, title, and message content
    //cy.get('.icon mat-icon').should('have.attr', 'ng-reflect-svg-icon', 'info');
    cy.get('.title').should('have.text', 'Attention');
    //cy.get('.alert-message').should('have.text', messageData.message);

    // Simulate clicking the cancel icon
    //cy.get('.cancel mat-icon').click();

    // Verify that the snackbar is closed
    //cy.get('.custom-snack-bar').should('not.exist');
  });
});
