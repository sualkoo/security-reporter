import {delay} from "rxjs";

describe('template spec', () => {
  beforeEach(() => {
    cy.visit('http://localhost:4200/project-search')
  }
    )
  it('h1', () => {
   cy.get('h1').should('contain', 'Search items');
  })

  it('search-input', () => {
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

    it('upload-ZIP', () => {

      cy.fixture('zle.zip').then(fileContent => {
        cy.get('input[type="file"]').attachFile({ fileContent, fileName: 'zle.zip', mimeType: 'application/zip' });

      });
      cy.get('button#upload_button').click();
    });

})
