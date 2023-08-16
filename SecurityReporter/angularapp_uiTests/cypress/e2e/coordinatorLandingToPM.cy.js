
describe('Pentester lands to project search after login test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://sda-playground.azurewebsites.net/');
    });
    it('passes', () => {
        testLandingClient()
    })

    function testLandingClient() {
        cy.get('.buttons-container > :nth-child(3) > .mdc-button > .mdc-button__label').click()
        cy.get('#mat-input-0').click().type('coordinator@coordinator.sk')
        cy.get('#mat-input-1').click().type('coordinator')
        cy.get('.login-form > :nth-child(3)').click()
        cy.url().should('include', '/list-projects')
    }
})