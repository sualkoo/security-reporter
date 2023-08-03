describe('My First Test', () => {
  it('Visits the initial project page', () => {
    cy.visit('localhost:4200/list-projects/')
    cy.contains('app is running!')
  })
})



describe('Navigation Test', () => {
  it('should navigate to /list-projects page when clicking on button', () => {
    // Visit the localhost:4200
    cy.visit('http://localhost:4200');

    // Find and click the button that should navigate to /list-projects
    cy.contains('PROJECT LISTING').click();

    // Verify if the redirection occurred correctly
    cy.url().should('include', '/list-projects');
  });
});
