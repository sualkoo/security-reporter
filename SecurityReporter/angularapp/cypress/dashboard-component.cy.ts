import { mount } from "cypress/angular";
import { DashboardComponent } from "../src/app/dashboard/dashboard.component";
import { HttpClientModule } from "@angular/common/http";


describe('dashboard-component.cy.ts', () => {
  it('playground', () => {
    Mount(1400, 800);
  })

 
})

function Mount(width: number, height: number) {
  cy.intercept('GET', 'https://localhost:7075/dashboard/Criticality', {
    statusCode: 200,
    fixture: 'charts/criticality-chart.json'
  }).as('responseCrit');

  cy.intercept('GET', 'https://localhost:7075/dashboard/Vulnerability', {
    statusCode: 200,
    fixture: 'charts/vulnerability-chart.json'
  }).as('response')

  cy.viewport(width, height);
  mount(DashboardComponent, {
    imports: [HttpClientModule]
  });

  cy.wait('@responseCrit').its('response.statusCode').should('equal', 200)
  cy.wait('@response').its('response.statusCode').should('equal', 200)

}
