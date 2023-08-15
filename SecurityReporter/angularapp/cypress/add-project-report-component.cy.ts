import { AddProjectReportComponent } from '../src/app/project-search/components/add-project-report/add-project-report.component'
import { mount } from 'cypress/angular';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule, MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

describe('Testing bigger AddProjectReport component', () => {
  beforeEach(() => {
    Mount(800,400);
  })

  it('Should correctly load component', () => {
    CheckElements();
    cy.get('div.drop-zone');
    cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'cloud_upload');
    cy.get('h4').should('contain', 'Drag and drop files here');
    cy.get('h4').should('contain', 'or');
  })

  it('Should upload file into componentt', () => {
    UploadValidZip();
  })

  it('Should return invalid zip popup message', () => {
    UploadZipChangedStructure();
  })

  it('Should return something went wrong popup message', () => {
    UploadZipChangedContent();
  })
})

describe('Testing smaller AddProjectReport component', () => {
  beforeEach(() => {
    Mount(450,300);
  })

  it('Should correctly load component', () => {
    CheckElements();
  })

  it('Should upload file into component', () => {
    UploadValidZip();
  })

  it('Should return invalid zip popup message', () => {
    UploadZipChangedStructure();
  })

  it('Should return something went wrong popup message', () => {
    UploadZipChangedContent();
  })
})

function Mount(width: number, height: number) {
  cy.viewport(width, height);
  mount(AddProjectReportComponent, {
    imports: [HttpClientModule, MatSnackBarModule, MatIconModule, NoopAnimationsModule],
    providers: [
      { provide: MAT_SNACK_BAR_DATA, useValue: {} },
      { provide: MatSnackBarRef, useValue: {} },
    ]
  });
}

function CheckElements() {
  cy.get('div.pl-2').should('exist').and('be.visible');
  cy.get('div.center').should('exist').and('be.visible');
  cy.get('div.center.pt-4.pb-4').should('exist').and('be.visible');
  cy.get('h4.left.upload-items').should('contain', 'Upload items').and('exist').and('be.visible');
  cy.get('button.btn.btn-primary').should('contain', 'Browse for file').and('exist').and('be.visible')
  cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip').and('exist').and('be.visible');
  cy.get('button#upload_button').should('contain', 'Upload').and('exist').and('be.disabled');
}
function CheckInsertedFile(fileName: string) {
  cy.get('div.single-file.d-flex.align-items-center').should('exist').and('be.visible');
  cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color')
    .should('contain', 'folder_zip').and('be.visible').and('exist');
  cy.get('p').should('contain', fileName).and('exist').and('be.visible')
  cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color')
    .should('contain', 'delete').and('exist').and('be.visible');
  cy.get('button#upload_button').should('contain', 'Upload').and('exist').and('be.visible').and('be.enabled');
}

function NoInsertedFile(fileName: string) {
  cy.get('div.single-file.d-flex.align-items-center').should('not.exist');
  cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color')
    .contains('folder_zip').should('not.exist');
  cy.get('p').contains(fileName).should('not.exist');
  cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color')
    .contains('delete').should('not.exist');
  cy.get('button#upload_button').should('not.be.enabled');
}

function ValidZipUploadRequest() {
  cy.intercept('POST', '/api/project-reports', {
    statusCode: 200,
    fixtures: 'valid-zip-response.json'
  }).as('valid_request');

  cy.get('button#upload_button').should('contain', 'Upload').and('be.enabled').click();

  cy.wait('@valid_request').its('response.statusCode').should('equal', 200)
}

function InvalidZipUploadRequest() {
  cy.intercept('POST', '/api/project-reports', {
    statusCode: 500,
    body: {
      error: {
        message: 'Something went wrong',
        //Tu dam kevinov z backendu pre toto konkretne zip
        details: ['Error detail 1', 'Error detail 2']
      }
    }
  }).as('valid_request');

  cy.get('button#upload_button').should('contain', 'Upload').and('be.enabled').click();

  cy.wait('@valid_request').its('response.statusCode').should('equal', 500)
}

function UploadValidZip() {
  cy.fixture('zip/dobre.zip').then(fileContent => {
    cy.get('input[type="file"]').attachFile({
      fileContent: fileContent,
      filePath: 'zip/dobre.zip',
      fileName: 'dobre.zip',
      mimeType: 'application/zip'
    })
  })
    CheckInsertedFile('dobre.zip');
    
    ValidZipUploadRequest();
    NoInsertedFile('dobre.zip');
    Popup('Succesfull', 'Report successfully saved to DB.', 'check');
}

function UploadZipChangedStructure() {
  cy.fixture('zip/zle.zip').then(fileContent => {
    cy.get('input[type="file"]').attachFile({
      fileContent: fileContent,
      filePath: 'zip/zle.zip',
      fileName: 'zle.zip',
      mimeType: 'application/zip'
    })
  }).then(() => {
    CheckInsertedFile('zle.zip');
    cy.get('button#upload_button').should('contain', 'Upload').and('be.enabled').click();
    NoInsertedFile('zle.zip');

    Popup('Warning', 'Zip file is incorrect', 'warning');
  })
}

function UploadZipChangedContent() {
  cy.fixture('zip/TimeFrameReportDue-non_date_value.zip').then(fileContent => {
    cy.get('input[type="file"]').attachFile({
      fileContent: fileContent,
      filePath: 'zip/TimeFrameReportDue-non_date_value.zip',
      fileName: 'TimeFrameReportDue-non_date_value.zip',
      mimeType: 'application/zip'
    })
  });
  CheckInsertedFile('TimeFrameReportDue-non_date_value.zip');
  InvalidZipUploadRequest();
  Popup('Something went wrong', '', 'error');
  NoInsertedFile('TimeFrameReportDue-non_date_value.zip');
}

function Popup(title: string, message: string, icon: string) {
  cy.get('div.title').should('contain', title);
  cy.get('div.alert-message').should('contain', message);
  cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', icon);
  cy.get('mat-icon.mat-icon.notranslate.cancel.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'close');
  cy.get('div#cdk-overlay-0.cdk-overlay-pane', { timeout: 6000 }).should('not.exist');
}






