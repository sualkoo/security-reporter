import { mount } from "cypress/angular";
import { DashboardComponent } from "../src/app/dashboard/dashboard.component";
import { HttpClientModule } from "@angular/common/http";


describe('dashboard-component.cy.ts', () => {
  //Niekedy nacita v pohode vsetky grafy, inokedy mi da exception idk why 
  it('playground', () => {
    Mount(1400, 800);
  })

 
})

function Mount(width: number, height: number) {
  

  cy.viewport(width, height);
  mount(DashboardComponent, {
    imports: [HttpClientModule]
  });

  cy.intercept('GET', '/api/dashboard/Criticality', {
    statusCode: 200,
    fixture: 'charts/criticality-chart.json'
  }).as('responseCrit');

  cy.intercept('GET', '/api/dashboard/Vulnerability', {
    statusCode: 200,
    fixture: 'charts/vulnerability-chart.json'
  }).as('response');
  
  cy.intercept('GET', '/api/dashboard/CWE', {
    statusCode: 200,
    fixture: 'charts/cwe-chart.json'
  }).as('responseCWE');

  /*cy.wait('@responseCrit').its('response.statusCode').should('equal', 200)
  cy.wait('@response').its('response.statusCode').should('equal', 200)
  cy.wait('@responseCWE').its('response.statusCode').should('equal', 200)*/
}
