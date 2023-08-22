describe('Item Management Test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://sda-projectmanagement.azurewebsites.net/');
        // Assuming the user is already logged in and on the homepage or item management section.
    });

    it('should edit a page', () => {
        navigateToListingProjects();
        //SearchSpecificPMItem();
        editPageVerification();
        editPageActions();
        errorHandling();
        feedbackWindow();
    });
                                                                        
    // Step 1: View the list or overview of items
    function navigateToListingProjects() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click();
    }

    // Step 2: Select the edit option for a specific item
    function SearchSpecificPMItem() {
        cy.get('.mat-expansion-panel-header-title > div').click();
        cy.get('.ng-tns-c1205077789-2 > .mat-mdc-form-field-infix').click();
        cy.get('#mat-input-0').type('A');
        cy.get(':nth-child(2) > app-select-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper').click();
        cy.get('#mat-option-9').click();
        cy.get(':nth-child(5) > app-datepicker > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        const clicks = 5;
        for (let i = 0; i < clicks; i++) {
            cy.get('.mat-calendar-previous-button').click();
        }
        cy.get('.cdk-overlay-backdrop').click();
        cy.get('.mat-mdc-row:nth-child(1) .mat-icon').contains('edit').click();
    }

    function editPageVerification() {
        // Step 3: Verify that the web application navigates to the edit page/form
        cy.get('.mat-mdc-row:nth-child(1) .mat-icon').contains('edit').click();
        cy.url().should('include', '/edit-project/');

        cy.wait(1000);

        // Step 4: Verify that the edit page/form is pre-populated with the existing item's information 
        //cy.get('#mat-select-value-13').should('not.be.empty');
        //cy.get('#mat-select-value-15').should('not.be.empty');
        //cy.get('#mat-select-value-17').should('not.be.empty');
        //cy.get('#mat-input-11').invoke('val').should('not.be.empty');
    }

    // Step 5: Modify the item's information accurately using input fields or controls
    function editPageActions() {
        cy.get('#mat-input-8').click({ force: true });
        cy.get('#mat-input-8').clear().type('dataNow', { force: true });
        cy.get('#mat-radio-3-input').click();
        cy.get('#mat-input-14').click().type('random@siemens-healthineers.com');
        cy.get(':nth-child(9) > .mdc-button__label').click();
        cy.get('#mat-select-value-13').click();
        cy.get('#mat-option-31').click();
    }

    // Step 6: Verify that mandatory fields are indicated and prevent incomplete data submission
    function errorHandling() {
        cy.wait(1000);
        cy.get('#mat-input-8').click().clear().type('SDA2023');
        cy.wait(1000);
        cy.get('.create-button > .mdc-button__label').click();

    }

    // Step 7: Verify that real-time feedback and validation messages are provided for errors or missing information

    function feedbackWindow() {
        cy.get('.mat-mdc-simple-snack-bar > .mat-mdc-snack-bar-label').should('be.visible');
    }
});