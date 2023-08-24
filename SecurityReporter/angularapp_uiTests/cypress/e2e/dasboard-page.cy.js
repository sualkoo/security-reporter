describe('Testing landing page - big viewport', () => {
    beforeEach(() => {
        cy.visit('https://localhost:4200/welcome');
        Login();
    })

    it('Dasboard loaded correctly', () => {
        CheckContent();
    })

    it('Redirect to Project Search page', () => {
        Redirect('Project Search', 'project-search');
    })

    it('Redirect to Project Management page', () => {
        Redirect('Project Management', 'project-management');
    })
})

describe('Testing landing page - small viewport', () => {
    beforeEach(() => {
        cy.viewport(600, 800);
        cy.visit('https://localhost:4200/welcome');
        LoginSmallComponent();
    })

    it('Dasboard loaded correctly', () => {
        CheckContent();
    })

    it('Redirect to Project Search page', () => {
        RedirectSmallComponent('Project Search', 'project-search');
    })

    it('Redirect to Project Management page', () => {
        RedirectSmallComponent('Project Management', 'project-management');
    })
})

function Login() {
    cy.get('button').contains('Login').click();
    cy.get('input#mat-input-0').type('admin@admin.sk')
    cy.get('input#mat-input-1').type('admin');
    cy.get('.login-form > :nth-child(3)').click()
    cy.get('a.nav-link').contains('Dashboard').click();
}

function LoginSmallComponent() {
    cy.get('span.navbar-toggler-icon').click().then(() => {
        Login();
    })
}

function Redirect(page, urlInclude) {
    cy.get('a.nav-link').contains(page).click().then(() => {
        cy.url().should('include', urlInclude);
        cy.get('a.nav-link').contains(page).should('have.class', 'active').and('have.class', 'fw-bolder');
        cy.get('a.nav-link').contains('Dashboard').should('not.have.class', 'active').and('not.have.class', 'fw-bolder');
    })
}

function RedirectSmallComponent(page, urlInclude) {
    cy.get('span.navbar-toggler-icon').click().then(() => {
        Redirect(page, urlInclude);
    })
}

function CheckContent() {
    cy.get('h1').should('contain', 'DASHBOARD').and('be.visible');
    cy.get('div.group-inside').should('exist').and('be.visible');
    cy.get('div.graph-group').should('exist').and('be.visible');
    cy.get('div.block').should('exist').and('be.visible').and('have.length', 2);
    cy.get('div.rectangle').should('exist').and('be.visible').and('have.length', 2);
    cy.get('canvas#criticalityChart').should('exist').and('be.visible');
    cy.get('canvas#vulnerabilityChart').should('exist').and('be.visible');
    cy.get('canvas#CWEChart.bar-chart').should('exist').and('be.visible');
    cy.get('canvas#CVSSChart.bar-chart').should('exist').and('be.visible');
}