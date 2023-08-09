describe('Landing page to listing projects page', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200/');
    });

    it('should navigate to ', () => {
        navigateToLogin();
    });

    function navigateToLogin() {
        cy.get('.buttons-container > :nth-child(3) > .mdc-button > .mdc-button__label').click()
        cy.get('#mat-input-0').click().type('admin')
        cy.get('#mat-input-1').click().type('admin')
        cy.get('.login-form > :nth-child(3)').click()
        cy.get('.buttons-container > :nth-child(3) > .mdc-button > .mdc-button__label').click()
        cy.get('.mat-mdc-simple-snack-bar > .mat-mdc-snack-bar-label').should('be.visible')
        cy.url().should('include', '/welcome')
    }
        
});