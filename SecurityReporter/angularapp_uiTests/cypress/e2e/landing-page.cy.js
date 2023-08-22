describe('Testing bigger LandingPageComponent', () => {
    beforeEach(() => {
        cy.visit('https://sda-playground.azurewebsites.net/');
    })

    it('Should mount component correctly', () => {
        CheckBanner();
        CheckContent();
        CheckFooterElements();
    })
})


describe('Testing smaller LandingPageComponent', () => {
    beforeEach(() => {
        cy.viewport(600, 800);
        cy.visit('https://sda-playground.azurewebsites.net/');
    })

    it('Should mount component correctly', () => {
        CheckBanner();
        CheckContent();
        CheckFooterElements();
    })
})

function CheckFooterLink(divNumber, linkNumber, text) {
    cy.get('div.col.col-xs-12.col-sm-6.col-md-4.col-lg-3.col-xl-3').eq(divNumber).children().children().eq(linkNumber).should($child => {
        expect($child).to.have.prop('tagName', 'LI').and.to.have.id('link_title').and.to.contain(text);
    });
}

function CheckFooterAElement(linkNumber, text) {
    cy.get('p#footer_more_info').children().eq(linkNumber).should($child => {
        expect($child).to.have.prop('tagName', 'A').and.to.have.class('footer-link').and.to.contain(text);
    })
}

function CheckFooterElements() {
    cy.get('footer.bg-dark.text-light.py-4').should('exist').and('be.visible');
    cy.get('div.container.pt-5').should('exist').and('be.visible');
    cy.get('div.col.col-xs-12.col-sm-6.col-md-4.col-lg-3.col-xl-3').should('exist').and('have.length', 6).and('be.visible');

    cy.get('h5#section_title').contains('Products & Services').should('exist').and('be.visible');
    cy.get('h5#section_title').contains('Support & Documentation').should('exist').and('be.visible');
    cy.get('h5#section_title').contains('Insights').should('exist').and('be.visible');
    cy.get('h5#section_title').contains('About Us').should('exist').and('be.visible');
    cy.get('h5#connect_section_title').contains('Connect').should('exist').and('be.visible');
    cy.get('h5#newsletter_section_title').contains('Newsletter').should('exist').and('be.visible');

    CheckFooterLink(0, 0, 'Value Partnerships & Consulting');
    CheckFooterLink(0, 1, 'Medical Imaging');
    CheckFooterLink(0, 2, 'Laboratory Diagnostics');
    CheckFooterLink(0, 3, 'Point of Care Testing');
    CheckFooterLink(0, 4, 'Digital Solutions & Automation');
    CheckFooterLink(0, 5, 'Services');
    CheckFooterLink(0, 6, 'Healthcare IT');
    CheckFooterLink(0, 7, 'Clinical Specialities & Diseases');

    CheckFooterLink(1, 0, 'Document Library (SDS, IFU, etc.)');
    CheckFooterLink(1, 1, 'Education & Training');
    CheckFooterLink(1, 2, 'PEPconnect');
    CheckFooterLink(1, 3, 'Teamplay Fleet');
    CheckFooterLink(1, 4, 'Webshop');
    CheckFooterLink(1, 5, 'Online Services');

    CheckFooterLink(2, 0, 'Innovating Personalized Care');
    CheckFooterLink(2, 1, 'Achieving Operational Excellence');
    CheckFooterLink(2, 2, 'Transforming the System of Care');
    CheckFooterLink(2, 3, 'Insights Center');

    CheckFooterLink(3, 0, 'About Siemens Healthineers');
    CheckFooterLink(3, 1, 'Compliance');
    CheckFooterLink(3, 2, 'Conferences & Events');
    CheckFooterLink(3, 3, 'Contact Us');
    CheckFooterLink(3, 4, 'Investor Relations');
    CheckFooterLink(3, 5, 'Job Search');
    CheckFooterLink(3, 6, 'Perspectives');
    CheckFooterLink(3, 7, 'Press Center');

    cy.get('ul.list-unstyled.horizontal-group-icon').should('exist').and('be.visible');
    cy.get('i.footer-icon.bi-twitter').should('exist').and('be.visible');
    cy.get('i.footer-icon.bi-facebook').should('exist').and('be.visible');
    cy.get('i.footer-icon.bi-instagram').should('exist').and('be.visible');
    cy.get('i.footer-icon.bi-linkedin').should('exist').and('be.visible');
    cy.get('i.footer-icon.bi-youtube').should('exist').and('be.visible');

    cy.get('button.btn-primary.m-1').contains('Subscribe').should('exist').and('be.enabled').and('be.visible');
    cy.get('p#footer_more_info').should('exist').and('be.visible');
}

function CheckBigNavbar() {
    /*cy.get('img.img-fluid').should('exist').and('be.visible');
    cy.get('button.btn.btn-link.d-flex.align-items-center.login').should('exist').and('be.enabled');
    cy.get('i').should('exist').and('be.visible');
  
    //Navbar
    cy.get('nav.navbar.navbar-expand-lg.navbar-dark.bg-dark').should('exist').and('be.visible');
    cy.get('div#navbarNav.collapse.navbar-collapse').should('exist').and('be.visible');
    cy.get('a.nav-link.link').contains('Home').should('exist').and('be.visible');
    cy.get('a.nav-link.link').contains('About us').should('exist').and('be.visible');
    cy.get('a.nav-link.link').contains('How to order a pentest').should('exist').and('be.visible');*/
}

function CheckSmallNavbar() {
    /*cy.get('nav.navbar.navbar-expand-lg.navbar-dark.bg-dark').should('exist').and('be.visible');
    cy.get('button.navbar-toggler.offset-1').should('exist').and('be.visible').and('be.enabled');
    cy.get('span.navbar-toggler-icon').should('exist').and('be.visible');
  
    cy.get('a.nav-link.link').contains('Home').should('not.be.visible');
    cy.get('a.nav-link.link').contains('About us').should('not.be.visible');
    cy.get('a.nav-link.link').contains('How to order a pentest').should('not.be.visible');
  
    cy.get('button.navbar-toggler.offset-1').click();
  
    cy.get('a.nav-link.link').contains('Home').should('exist').and('be.visible');
    cy.get('a.nav-link.link').contains('About us').should('exist').and('be.visible');
    cy.get('a.nav-link.link').contains('How to order a pentest').should('exist').and('be.visible');*/
}

function CheckContent() {
    cy.get('div.row.pt-4.mt-4').should('exist').and('have.length', 3);
    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').should('exist').and('have.length', 6);
    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(0).children().eq(0).should('have.class', 'img-fluid');
    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(1).children().eq(0).should('contain', 'What is pentest?')
    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(1).children().eq(1).should($element => {
        expect($element).to.have.prop('tagName', 'P');
    });

    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(2).children().eq(1).should($element => {
        expect($element).to.have.prop('tagName', 'P');
    });

    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(3).children().eq(0).should($element => {
        expect($element).to.have.prop('tagName', 'CANVAS');
        expect($element).to.have.id('criticalityChart');
    });

    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(4).children().eq(0).should($element => {
        expect($element).to.have.prop('tagName', 'CANVAS');
        expect($element).to.have.id('vulnerabilityChart');
    })

    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(5).children().eq(0).should('contain', 'What is Vulnerability?')
    cy.get('div.offset-1.col-11.col-xl-4.col-lg-4').eq(5).children().eq(1).should($element => {
        expect($element).to.have.prop('tagName', 'P');
    });
}

function CheckBanner() {
    cy.get('section.hero-block.overflow-hidden.width-100').should('exist').and('be.visible');
    cy.get('div.width-100.bg-color-black').should('exist').and('be.visible');
    cy.get('div.hero-block__content').should('exist').and('be.visible');
    cy.get('div.row.justify-content-end').should('exist').and('be.visible');
    cy.get('h1').should('exist').and('be.visible');
    cy.get('span.hero-block__title.display-block').contains('Cybersecurity Assesment').should('exist').and('be.visible');
    cy.get('span.h5.hero-block__subtitle.display-block').contains('(1337 Pentesters)').should('exist').and('be.visible');
    cy.get('div.col-12.col-lg-6.order-lg-2').should('exist').and('be.visible');
    cy.get('img.img-fluid').should('exist').and('be.visible');
}

