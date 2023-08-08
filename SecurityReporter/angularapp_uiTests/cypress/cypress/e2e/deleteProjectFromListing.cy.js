describe('Add incorrect item test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200/');
    });
    it('passes', () => {
        testDeleteProject()
    })

    function testDeleteProject() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click()
        cy.get('#mat-mdc-checkbox-2-input').click()
        cy.get('.delete-button > .mdc-button__label').click()
        cy.get('.delete-btn > .mdc-button__label').click()
    }
})