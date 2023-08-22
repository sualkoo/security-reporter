describe('Item Management Test', () => {
    beforeEach(() => {
        cy.viewport(1280, 720);
        cy.visit('https://sda-projectmanagement.azurewebsites.net/');        
    });

    it('should add project', () => {
        navigateToAddProjectPage();
        fillInputValues();
        fillSelectableFields();
        fillRadioButtonValues();
        fillDateValues();
        addProject();
        feedbackWindow();
    });

    function navigateToAddProjectPage() {
        cy.get('.buttons-container > :nth-child(1) > .mdc-button__label').click();
        cy.get('.button-container > .add-button').click();
    }

    function fillInputValues() {
        cy.get('#mat-input-8').click().type("Testing");
        cy.get('#mat-input-9').click().type("Testing");
        cy.get('#mat-input-10').click().type("Testing");
        cy.get('#mat-input-11').click().type("Testing");
        cy.get('#mat-input-12').click().type("Testing");
        cy.get(':nth-child(4) > .mdc-button__label').click();
        cy.get('#mat-input-13').click().type("Testing");
        cy.get('#mat-input-14').click().type('random@siemens-healthineers.com');
        cy.get(':nth-child(9) > .mdc-button__label').click();
    }

    function fillSelectableFields() {
        cy.get(':nth-child(2) > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix').click();
        cy.get('#mat-option-30').click();

        cy.get(':nth-child(3) > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix').click();
        cy.get('#mat-option-36').click();

        cy.get(':nth-child(4) > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix').click();
        cy.get('#mat-option-39').click();

        cy.get(':nth-child(2) > app-select-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-infix').click();
        cy.get('#mat-option-46').click();
    }

    function fillRadioButtonValues() {
        cy.get('#mat-radio-3-input').click();
    }

    function fillDateValues() {
        cy.get(':nth-child(1) > :nth-child(1) > :nth-child(1) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        cy.get(':nth-child(5) > [data-mat-col="2"] > .mat-calendar-body-cell').click();

        cy.get(':nth-child(1) > :nth-child(2) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        cy.get(':nth-child(5) > [data-mat-col="3"] > .mat-calendar-body-cell').click();

        cy.get(':nth-child(2) > :nth-child(1) > :nth-child(1) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        cy.get(':nth-child(6) > [data-mat-col="0"] > .mat-calendar-body-cell').click();

        cy.get(':nth-child(7) > :nth-child(1) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        cy.get(':nth-child(5) > [data-mat-col="3"] > .mat-calendar-body-cell').click();

        cy.get('.pm-background').click({ force: true });

        cy.get(':nth-child(7) > :nth-child(2) > datepicker-component > .mat-mdc-form-field > .mat-mdc-text-field-wrapper > .mat-mdc-form-field-flex > .mat-mdc-form-field-icon-suffix').click();
        cy.get(':nth-child(5) > [data-mat-col="3"] > .mat-calendar-body-cell').click();

        cy.get('.pm-background').click();
    }

    function addProject() {
        cy.get('.create-button > .mdc-button__label').click();
    }

    function feedbackWindow() {
        cy.get('.mat-mdc-simple-snack-bar > .mat-mdc-snack-bar-label').should('be.visible');
    }
});