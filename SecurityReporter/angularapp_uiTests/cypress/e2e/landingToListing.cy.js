describe('Landing page to listing projects page', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200/');
    });

    it('should navigate to ', () => {
        navigateToAddProjectPage();
        
    });

    function navigateToAddProjectPage() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click();
        cy.url().should('include', '/list-projects');
    }
});