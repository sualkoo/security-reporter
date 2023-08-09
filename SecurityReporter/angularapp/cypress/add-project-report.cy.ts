import { AddProjectReportComponent } from '../src/app/project-search/components/add-project-report/add-project-report.component'
import { mount } from 'cypress/angular';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule, MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';

describe('add-project-report.cy.ts', () => {

  it.skip('Should correctly load component for bigger viewport', () => {
    MountBiggerViewport();
    cy.get('h4.left.upload-items').should('contain', 'Upload items');
    cy.get('div.drop-zone');
    cy.get('mat-icon.mat-icon.notranslate.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'cloud_upload');
    cy.get('h4').should('contain','Drag and drop files here');
    cy.get('h4').should('contain', 'or');
    cy.get('button.btn.btn-primary').should('contain', 'Browse for file');
    cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip');
    cy.get('button#upload_button').should('contain', 'Upload');
  })

  it.skip('Should correctly load component for smaller viewport', () => {
    MountSmallerViewport();
    cy.get('h4.left.upload-items').should('contain', 'Upload items');
    cy.get('button.btn.btn-primary').should('contain', 'Browse for file');
    cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip');
    cy.get('button#upload_button').should('contain', 'Upload');
  })

  it('Should upload file into component with big viewport', () => {
    MountBiggerViewport();

    //Doesnt attach it idk why
    cy.get('input[type="file"]').attachFile({
      filePath: 'dobre.zip',
      fileName: 'dobre.zip',
      mimeType: 'application/zip'
    }).then(() => {
      cy.wait(2000);
      cy.get('button#upload_button').click();
    });
    cy.wait(2000);
  })

  function MountBiggerViewport() {
    cy.viewport(800, 400);
    mount(AddProjectReportComponent, {
      imports: [HttpClientModule, MatSnackBarModule, MatIconModule],
      providers: [
        { provide: MAT_SNACK_BAR_DATA, useValue: {} },
        { provide: MatSnackBarRef, useValue: {} },
      ]
    });
  }

  function MountSmallerViewport() {
    cy.viewport(450, 300);
    mount(AddProjectReportComponent, {
      imports: [HttpClientModule, MatSnackBarModule],
      providers: [
        { provide: MAT_SNACK_BAR_DATA, useValue: {} },
        { provide: MatSnackBarRef, useValue: {} },
      ]
    });

  }

})


