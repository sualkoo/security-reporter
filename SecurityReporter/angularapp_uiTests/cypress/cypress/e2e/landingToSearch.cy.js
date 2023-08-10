describe('Landing page to listing projects page', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://sda-playground.azurewebsites.net/');
    });

    it('should navigate to ', () => {
        navigateToAddProjectPage();
    });

    function navigateToAddProjectPage() {
        cy.get(':nth-child(2) > .mdc-button > .mdc-button__label').click();
        cy.url().should('include', '/project-search');
    }
});