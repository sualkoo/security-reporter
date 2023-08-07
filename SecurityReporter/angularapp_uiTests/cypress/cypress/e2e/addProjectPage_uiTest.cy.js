describe('Item Management Test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200');        
    });

    it('should edit a page', () => {
        navigateToAddProjectPage();
        testrpojectName();
    });

    // Step 1: View the list and click Add Project button
    function navigateToAddProjectPage() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click();
        cy.get('.button-container > .add-button').click();
    }

    function testrpojectName() {
        cy.get('#mat-input-8').click().type("Test");
        cy.get(':nth-child(1) > :nth-child(1) > :nth-child(1) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix > .mat-datepicker-toggle > .mdc-icon-button > .mat-mdc-button-touch-target').click();
        
    }
});