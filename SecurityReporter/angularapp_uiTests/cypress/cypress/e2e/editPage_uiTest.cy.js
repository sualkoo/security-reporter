// Describe the test suite
describe('Edit Page Test', () => {
    // Set up the test environment before each test case
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://localhost:4200/list-projects');
    });

    // Test case: should edit a page
    it('should edit a page', () => {
        navigateToEditPage();
        editPageActions();
        saveChanges();
        verifyChanges();
    });

    // Function to navigate to the edit page
    function navigateToEditPage() {
        // Click on the edit icon to navigate to the edit page
        cy.get('.mat-icon').click();
        // Click on the form field to focus on it
        cy.get('.ng-tns-c1205077789-2 > .mat-mdc-form-field-infix').click();
        // Type 'A' into the input field
        cy.get('#mat-input-0').type('A');
        // Click on the edit icon of the first row to open the edit page
        cy.get('.mat-mdc-row:nth-child(1) .mat-icon').contains('edit').click();
        // Scroll to a specific position on the page
        cy.scrollTo(0, 260);
    }

    // Function to perform edit page actions
    function editPageActions() {
        // Click on the input field for Project Name and type 'VantedCorp'
        cy.get('#mat-input-8').click({ force: true });
        cy.get('#mat-input-8').clear().type('VantedCorp', { force: true });
        // Click on a button to perform some action
        cy.get('.ng-tns-c1205077789-17 .mat-mdc-button-touch-target').click();
        // Click on a specific cell in the calendar
        cy.get(':nth-child(4) > [data-mat-col="3"] > .mat-calendar-body-cell').click({ force: true });
        // Click on the body of the page to dismiss any popups or dropdowns
        cy.get('body').click();
        // Click on a dropdown and select an option
        cy.get('#mat-select-value-15').click();
        cy.get('#mat-option-35').click();
        // Click on a radio button to select it
        cy.get('#mat-radio-2-input').click();
        // Scroll to a specific position on the page
        cy.scrollTo(0, 260);
        // Click on an input field and type an email address
        cy.get('#mat-input-14').click().type('random@siemens-healthineers.com').should('have.value', 'random@siemens-healthineers.com');
        // Click on a button to perform some action
        cy.get(':nth-child(9) > .mdc-button__label').click();
        // Scroll to a specific position on the page
        cy.scrollTo(0, 308);
        // Click on a button to perform some action
        cy.get('.create-button > .mdc-button__label').click();
    }

    // Function to save changes (scroll to a specific position)
    function saveChanges() {
        cy.scrollTo(0, 75);
    }

    // Function to verify the changes made on the page
    function verifyChanges() {
        // Assert that the Project Name input field has the expected value 'VantedCorp'
        cy.get('#mat-input-8').should('have.value', 'VantedCorp');
        // Assert that a radio button is checked
        cy.get('#mat-radio-2-input').should('be.checked');
    }
});
