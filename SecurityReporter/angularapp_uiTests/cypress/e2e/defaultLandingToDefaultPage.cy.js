
describe('Client lands to project listing after login test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://sda-playground.azurewebsites.net/');
    });
    it('passes', () => {
        testLandingClient()
    })

    function testLandingClient() {
        cy.get('.buttons-container > :nth-child(3) > .mdc-button > .mdc-button__label').click()
        cy.get('#mat-input-0').click().type('default@default.sk')
        cy.get('#mat-input-1').click().type('default')
        cy.get('.login-form > :nth-child(3)').click()
        cy.url().should('include', '/default-page')
    }
})