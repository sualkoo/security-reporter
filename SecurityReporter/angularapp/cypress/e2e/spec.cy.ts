import {delay} from "rxjs";

describe('template spec', () => {
  beforeEach(() => {
    cy.visit('https://127.0.0.1:4200/project-search')
  })

  it('All elements exists after page loads', () => {
    //Search
    cy.get('h1').should('contain', 'Search items');
    cy.get('mat-form-field.search-field input[placeholder*="Search..."]');
    cy.get('h4').should('contain', 'Filters');
    cy.get('div.filter-item.ng-star-inserted');
    cy.get('label.p-2').first().should('contain', 'Project Name');
    cy.get('label.p-2').eq(1).should('contain', 'Details');
    cy.get('label.p-2').eq(2).should('contain', 'Impact');
    cy.get('label.p-2').eq(3).should('contain', 'Repeatability');
    cy.get('label.p-2').eq(4).should('contain', 'References');
    cy.get('label.p-2').eq(5).should('contain', 'CWE');
    cy.get('button#search_button').should('contain', 'Search');
    cy.get('button#clear_button').should('contain', 'Clear filters');

    //Upload
    cy.get('h4.left.upload-items').should('contain', 'Upload items');
    cy.get('div.pb-2.pt-1');
    cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'cloud_upload');
    cy.get('h4').should('contain', 'Drag and drop files here');
    cy.get('h4').should('contain', 'or');
    cy.get('button.btn.btn-primary').should('contain', 'Browse for file');
    cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip');
    cy.get('button#upload_button.btn.btn-secondary').should('contain', 'Upload');
  })

  it('Search elements exist after search', () => {
    //Check possible only after initial search for projects
    const searchTerm = 'dum';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').first().check();
    cy.get('input.checkbox').first().should('be.checked');

    cy.get('button#search_button').click();

    //Check if project elements exist
    cy.get('div.card-body');
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2');
    cy.get('h3.mb-2.ng-star-inserted');
    cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'menu');
    cy.get('mat-icon#upButton').should('not.exist');
  })

  it('Should have results after search', () => {
    const searchTerm = 'dum';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').first().check();
    cy.get('input.checkbox').first().should('be.checked');

    cy.get('button#search_button').click();
    cy.get('div.card-body');
    cy.get('mat-icon#upButton').should('not.exist');
  })

  it('Arrow button should appear after scrolling', () => {
    const searchTerm = 'dum';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').first().check();

    cy.get('button#search_button').click();

    cy.get('div#scrollable_box').scrollTo(0, 500);
    cy.get('mat-icon#upButton').should('contain', 'arrow_upward');
    
    cy.get('mat-icon#upButton').click();

  })

  it('Shouldnt have results after search', () => {
    const searchTerm = 'egnegoiuenroinwobgnwuigbwuio';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').first().check();
    cy.get('input.checkbox').first().should('be.checked');

    cy.get('button#search_button').click();

    cy.get('div.card-body').should('not.exist');
    cy.get('mat-icon#upButton').should('not.exist');
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').should('not.exist');
    cy.get('h3.mb-2.ng-star-inserted').should('not.exist');
  })

  it('CWE filters doesnt work after wrong input', () => {
    const searchTerm = 'egnegoiuenroinwobgnwuigbwuio';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').eq(5).check();
    cy.get('input.checkbox').eq(5).should('be.checked');

    cy.get('button#search_button').click();

    cy.get('div.card-body').should('not.exist');
    cy.get('mat-icon#upButton').should('not.exist');
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').should('not.exist');
    cy.get('h3.mb-2.ng-star-inserted').should('not.exist');
  })

  it('Should delete project', () => {
    const searchTerm = 'dum';

    cy.get('mat-form-field.search-field input').first().type(searchTerm);
    cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

    cy.get('input.checkbox').eq(0).check();
    cy.get('input.checkbox').eq(0).should('be.checked');

    cy.get('button#search_button').click();

    cy.get('div.card-body');
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2');
    cy.get('h3.mb-2.ng-star-inserted');
    cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'menu');

    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').first().click();
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').should('be.checked');

    cy.get('mat-icon#deleteButton').should('contain', 'delete_outline');
    cy.get('mat-icon#deleteButton').click();

    cy.get('div.popup');
    cy.get('b').should('contain', 'Selected projects to delete:');


    cy.get('mat-icon#confirmDeleteButton').should('contain', 'delete_outline');
    cy.get('mat-icon#confirmDeleteButton').click();
  })

  it('Search process', () => {
      const searchTerm = 'a';

      cy.get('mat-form-field.search-field input').first().type(searchTerm);
      cy.get('mat-form-field.search-field input').should('have.value', searchTerm);

      cy.get('input.checkbox').first().click();
      cy.get('input.checkbox').eq(1).click();
      cy.get('input.checkbox').eq(2).click();
      cy.get('input.checkbox').eq(3).click();
      cy.get('input.checkbox').eq(4).click();
      cy.get('input.checkbox').eq(5).click();

      cy.get('button#search_button').click();

      cy.get('input.checkbox').eq(1).click();

      cy.get('button#clear_button').click();
    }
    )

    it('Uploading wrong zip', () => {

      cy.fixture('zle.zip').then(fileContent => {
        cy.get('input[type="file"]').attachFile({ fileContent, fileName: 'zle.zip', mimeType: 'application/zip' });

      });
      cy.get('button#upload_button').click();
    });

})
