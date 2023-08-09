describe('Add incorrect item test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200/');
    });
  it('passes', () => {
      testProjectName()
  })

    function testProjectName() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click()
        cy.get('.add-button > .mdc-button__label').click()
        cy.get('#mat-input-8').click().type("aa")
        cy.get('#mat-mdc-error-1').should('be.visible');
    }
})