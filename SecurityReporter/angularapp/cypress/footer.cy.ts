import { mount } from "cypress/angular";
import { FooterComponent } from "../src/app/footer/footer.component";

describe('footer.cy.ts', () => {
  function MountViewport() {
    cy.viewport(500, 500);
    mount(FooterComponent, {

    });
  }

  it('All footer sections contain correct heading title content', () => {
    MountViewport();
    cy.get('h5#section_title').first().should('contain', 'Products & Services');
    cy.get('h5#section_title').eq(1).should('contain', 'Support & Documentation');
    cy.get('h5#section_title').eq(2).should('contain', 'Insights');
    cy.get('h5#section_title').eq(3).should('contain', 'About Us');
    cy.get('h5#connect_section_title').first().should('contain', 'Connect');
    cy.get('h5#newsletter_section_title').should('contain', 'Newsletter');
  })

  it('"Products & Services" section contains correct title content', () => {
    MountViewport();
    cy.get('li#link_title').first().should('contain', 'Value Partnerships & Consulting');
    cy.get('li#link_title').eq(1).should('contain', 'Medical Imaging');
    cy.get('li#link_title').eq(2).should('contain', 'Laboratory Diagnostics');
    cy.get('li#link_title').eq(3).should('contain', 'Point of Care Testing');
    cy.get('li#link_title').eq(4).should('contain', 'Digital Solutions & Automation');
    cy.get('li#link_title').eq(5).should('contain', 'Services');
    cy.get('li#link_title').eq(6).should('contain', 'Healthcare IT');
    cy.get('li#link_title').eq(7).should('contain', 'Clinical Specialities & Diseases');
  })

  it('"Support & Documentation" section contains correct title content', () => {
    MountViewport();
    cy.get('li#link_title').eq(8).should('contain', 'Document Library (SDS, IFU, etc.)');
    cy.get('li#link_title').eq(9).should('contain', 'Education & Training');
    cy.get('li#link_title').eq(10).should('contain', 'PEPconnect');
    cy.get('li#link_title').eq(11).should('contain', 'Teamplay Fleet');
    cy.get('li#link_title').eq(12).should('contain', 'Webshop');
    cy.get('li#link_title').eq(13).should('contain', 'Online Services');
  })

  it('"Insights" section contains correct title content', () => {
    MountViewport();
    cy.get('li#link_title').eq(14).should('contain', 'Innovating Personalized Care');
    cy.get('li#link_title').eq(15).should('contain', 'Achieving Operational Excellence');
    cy.get('li#link_title').eq(16).should('contain', 'Transforming the System of Care');
    cy.get('li#link_title').eq(17).should('contain', 'Insights Center');
  })

  it('"About Us" section contains correct title content', () => {
    MountViewport();
    cy.get('li#link_title').eq(18).should('contain', 'About Siemens Healthineers');
    cy.get('li#link_title').eq(19).should('contain', 'Compliance');
    cy.get('li#link_title').eq(20).should('contain', 'Conferences & Events');
    cy.get('li#link_title').eq(21).should('contain', 'Contact Us');
    cy.get('li#link_title').eq(22).should('contain', 'Investor Relations');
    cy.get('li#link_title').eq(23).should('contain', 'Job Search');
    cy.get('li#link_title').eq(24).should('contain', 'Perspectives');
    cy.get('li#link_title').eq(25).should('contain', 'Press Center');
  })

  it('Icon in "Connect" section are visible', () => {
    MountViewport();
    cy.get('i.footer-icon.bi-twitter').should('be.visible');
    cy.get('i.footer-icon.bi-facebook').should('be.visible');
    cy.get('i.footer-icon.bi-instagram').should('be.visible');
    cy.get('i.footer-icon.bi-linkedin').should('be.visible');
    cy.get('i.footer-icon.bi-youtube').should('be.visible');
  })

  it('Button in "Newsletter" contains correct value and is visible', () => {
    MountViewport();
    cy.get('.btn-primary.btn-primary.m-1').should('be.visible');
    cy.get('.btn-primary.btn-primary.m-1').should('contain', 'Subscribe');
  })

  it('More info in footer is visible', () => {
    MountViewport();
    cy.get('p#footer_more_info').should('be.visible');
  })
})
