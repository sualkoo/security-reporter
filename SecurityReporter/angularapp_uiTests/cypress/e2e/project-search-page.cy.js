describe('template spec', () => {
    //Should add tests for navbar

    beforeEach(() => {
        cy.visit('http://localhost:4200/project-search');
    })

    it('Should correctly load component', () => {
        //Search and Filters
        CheckElements();

        //Upload
        CheckUploadElementsBiggerViewport();
    })

    it('CWE shouldnt be avalaible with string in search bar', () => {
        CWECheck();
        Popup('Warning', 'If you choose CWE, search value has to be a number', 'warning');
    })

    it('Clear filters should be visible only when filters selected', () => {
        ClearFiltersButtonCheck();
    })

    it('Selecting single filter should enable clear button', () => {
        SingleFilterClearButton();
    })

    it('Should return search items', () => {
        SearchRequest();

        //Check if elements after load exist
        CheckElementsAfterSearch();
    })

    it('Should hide sidebar', () => {
        SearchRequest();
        cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color.ng-star-inserted').should('contain', 'menu').and('exist').click();
        CheckNoSidebarElements();
        cy.get('div.drop-zone.ng-star-inserted').should('not.be.visible');
        cy.get('mat-icon').contains('cloud_upload').should('not.be.visible');
        cy.get('h4').contains('Drag and drop files here').should('not.be.visible');
        cy.get('h4').contains('or').should('not.be.visible');

        CheckElementsAfterSearch();
    })

    it('Shouldnt return search items', () => {
        NoSearchItems();
    })

    it('Should "delete" item ', () => {
        SearchRequest();
        CheckDeleteElements();
        DeleteItems();
    })

    it('Should "delete" item - no sidebar', () => {
        SearchRequest();
        cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color.ng-star-inserted').should('contain', 'menu').and('exist').click();
        CheckDeleteElements();
        DeleteItems();
    })
})

describe('Project search page components tests - smaller viewport', () => {
    beforeEach(() => {
        cy.viewport(400, 800);
        cy.visit('http://localhost:4200/project-search');
    })

    it('Should correctly load component', () => {
        //Search and Filters
        CheckElements();

        //Upload
        CheckUploadElementsSmallerViewport();
    })

    it('CWE shouldnt be avalaible with string in search bar', () => {
        CWECheck();
        Popup('Warning', 'If you choose CWE, search value has to be a number', 'warning');
    })

    it('Clear filters should be visible only when filters selected', () => {
        ClearFiltersButtonCheck();
    })

    it('Selecting single filter should enable clear button', () => {
        SingleFilterClearButton();
    })

    it('Shouldnt return search items', () => {
        NoSearchItems();
    })

    it('Should return search items', () => {
        SearchRequest();

        cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color.ng-star-inserted').should('contain', 'menu').and('exist').click();
        CheckElementsAfterSearch();
        CheckNoSidebarElements();

        cy.get('div.drop-zone.ng-star-inserted').should('not.exist');
        cy.get('mat-icon').contains('cloud_upload').should('not.exist');
        cy.get('h4').contains('Drag and drop files here').should('not.exist');
        cy.get('h4').contains('or').should('not.exist');
    })

    it('Should "delete" item', () => {
        SearchRequest();
        cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color.ng-star-inserted').should('contain', 'menu').and('exist').click();
        CheckDeleteElements();
        DeleteItems();
    })
})

//Functions
function CheckElements() {
    CheckExistenceOfSearchElements();
    CheckExistenceOfFiltersElements();
}

function CheckExistenceOfSearchElements() {
    cy.get('div.col-auto').should('be.visible');
    cy.get('div.mat-mdc-text-field-wrapper');
    cy.get('mat-icon').should('contain', 'search');
    cy.get('input[placeholder="Search..."]').should('be.visible');
}

function CheckExistenceOfFiltersElements() {
    cy.get('h4').should('contain', 'Filters').and('be.visible');
    cy.get('label.p-2').first().should('contain', 'Project Name').and('be.visible');
    cy.get('label.p-2').eq(1).should('contain', 'Details').and('be.visible');
    cy.get('label.p-2').eq(2).should('contain', 'Impact').and('be.visible');
    cy.get('label.p-2').eq(3).should('contain', 'Repeatability').and('be.visible');
    cy.get('label.p-2').eq(4).should('contain', 'References').and('be.visible');
    cy.get('label.p-2').eq(5).should('contain', 'CWE').and('be.visible');
    cy.get('input.checkbox').first().should('be.visible')
    cy.get('input.checkbox').eq(1).should('be.visible');
    cy.get('input.checkbox').eq(2).should('be.visible');
    cy.get('input.checkbox').eq(3).should('be.visible');
    cy.get('input.checkbox').eq(4).should('be.visible');
    cy.get('input.checkbox').eq(5).should('be.visible');
    cy.get('div.center.submit-button.pt-4').should('be.visible');
    cy.get('button#search_button').should('contain', 'Search').and('be.disabled');
    cy.get('button#clear_button').should('contain', 'Clear').and('be.disabled');
}

function CheckUploadElementsBiggerViewport() {
    cy.get('div.pl-2');
    cy.get('h4.left.upload-items');
    cy.get('div.drop-zone.ng-star-inserted');
    cy.get('div.pb-2.pt-1');
    cy.get('mat-icon').should('contain', 'cloud_upload').and('be.visible');
    cy.get('h4').should('contain', 'Drag and drop files here').and('be.visible');
    cy.get('h4').should('contain', 'or').and('be.visible');
    cy.get('button.btn.btn-primary').should('contain', 'Browse for file').and('be.visible').and('be.enabled');
    cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip');
    cy.get('div.center.pt-4.pb-4');
    cy.get('button.btn.btn-secondary').should('contain', 'Upload').and('be.disabled');
}

function CheckUploadElementsSmallerViewport() {
    cy.get('div.pl-2');
    cy.get('h4.left.upload-items').and('be.visible');
    cy.get('div.pb-2.pt-1');
    cy.get('div.center');
    cy.get('button.btn.btn-primary').should('contain', 'Browse for file').and('be.visible').and('be.enabled');
    cy.get('p.file-type.pt-1').should('contain', 'Accepted file type: .zip');
    cy.get('div.center.pt-4.pb-4');
    cy.get('button.btn.btn-secondary').should('contain', 'Upload').and('be.disabled');
}

function CheckOneFilter(order) {
    cy.get('input.checkbox').eq(order).then(() => {
        cy.get('input.checkbox').eq(order).check();
        cy.get('input.checkbox').eq(order).should('be.checked');
        cy.get('button#clear_button').should('contain', 'Clear').and('be.enabled');
        cy.get('button#clear_button').should('contain', 'Clear').click();
        cy.get('button#clear_button').should('contain', 'Clear').and('be.disabled');
        cy.get('input.checkbox').should('not.be.checked');
    })
}

function CheckElementsAfterSearch() {
    cy.get('div#scrollable_box').should('exist').and('be.visible');
    cy.get('h4.ng-star-inserted').should('contain', 'Found findings: 21').and('be.visible');
    cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color.ng-star-inserted').should('contain', 'menu').and('exist');

    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').should('be.visible');
    cy.get('div.card-body').should('be.visible').and('have.length', 24);
    cy.get('h3.mb-2.ng-star-inserted').should('contain', 'Dummy Project 1').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: DummyApplication Signed with a Debug Certificate').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: Missing Enforced Updating').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: Heap Inspection of Sensitive Memory').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: Outdated Components').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: ePHI is stored on device without encryption').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: Weak Application Signature').and('be.visible');
    cy.get('h5.ng-star-inserted').should('contain', 'Finding: Sensitive Information Disclosure via Logging').and('be.visible');

    cy.get('div#scrollable_box').scrollTo(0, 500).then(() => {
        cy.get('mat-icon#up_button').should('contain', 'arrow_upward').and('be.visible');
    })

    cy.get('mat-icon#up_button').should('contain', 'arrow_upward').click().then(() => {
       cy.get('mat-icon#up_button').should('not.exist');
    })

    cy.get('button.btn.btn-dark').should('contain', 'Get Source').and('exist').and('be.enabled');
    cy.get('button.btn.btn-light').should('contain', 'Get PDF').and('exist').and('be.enabled');
}

function CheckDeleteElements() {
    cy.get('mat-icon#delete_button').should('not.exist');
    cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').first().check().then(() => {
        cy.get('input.projectSelectionCheckbox.form-check-input.me-2.mt-2').first().should('be.checked').and('be.visible');

        cy.get('mat-icon#delete_button').should('contain', 'delete_outline').and('be.visible').and('exist');
        cy.get('mat-icon#delete_button').should('contain', 'delete_outline').click().then(() => {
            cy.get('div.popup').should('be.visible').and('exist');
            cy.get('mat-icon.mat-icon.notranslate.close-icon.close-popup-button.mat-warn.material-icons.mat-ligature-font').should('be.visible').and('exist');
            cy.get('div.container-fluid.bg-white.border').should('be.visible').and('exist');
            cy.get('h5').should('contain', 'Selected projects to delete: 1').and('be.visible');
            cy.get('b').should('contain', 'Selected projects to delete').and('be.visible');
            cy.get('b').should('contain', 'Project name:').and('be.visible');
            cy.get('b').should('contain', 'id:').and('be.visible');
            cy.get('p').should('contain', 'Project name: Dummy Project 1 id: 17dfdb47-04d4-46cd-91de-c92bea74b7d1').and('be.visible');
            cy.get('mat-icon.mat-icon.notranslate.icon.delete-selected-findings-button.material-icons.mat-ligature-font.mat-icon-no-color').should('be.visible').and('contain', 'delete_outline');
        })
    });
}

function CheckNoSidebarElements() {
    cy.get('mat-icon').contains('search').should('not.be.visible');;
    cy.get('div.col-auto').should('not.be.visible');
    cy.get('div.mat-mdc-text-field-wrapper').should('not.be.visible');
    cy.get('input[placeholder="Search..."]').should('not.be.visible');
    cy.get('h4').contains('Filters').should('not.be.visible');
    cy.get('label.p-2').should('not.be.visible');
    cy.get('input.checkbox').should('not.be.visible');
    cy.get('div.center.submit-button.pt-4').should('not.be.visible');
    cy.get('button#search_button').should('not.be.visible');
    cy.get('button#clear_button').should('not.be.visible');
    cy.get('div.pl-2').should('not.be.visible');
    cy.get('h4.left.upload-items').contains('Upload items').should('not.be.visible');
    cy.get('div.pb-2.pt-1').should('not.be.visible');
    cy.get('button.btn.btn-primary').should('not.be.visible');
    cy.get('p.file-type.pt-1').should('not.be.visible');
    cy.get('div.center.pt-4.pb-4').should('not.be.visible');
    cy.get('button.btn.btn-secondary').should('not.be.visible');
    cy.get('div.center').should('not.be.visible');
}

function SearchRequest() {
    cy.get('input[placeholder = "Search..."]').type('dum');
    cy.get('input.checkbox').first().should('be.visible').check();
    cy.get('input.checkbox').should('be.checked');

    cy.intercept('GET', `/api/project-reports/findings?page=1&projectName=dum&details=&impact=&repeatability=&references=&cwe=`, {
        statusCode: 200,
        fixture: 'search-response.json'
    }).as('response')

    cy.get('button#search_button').click();

    cy.wait('@response').its('response.statusCode').should('equal', 200)
}

function Popup(title, message, icon) {
    cy.get('div.title').should('contain', title);
    cy.get('div.alert-message').should('contain', message);
    cy.get('mat-icon.mat-icon.notranslate.icon.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', icon);
    cy.get('mat-icon.mat-icon.notranslate.cancel.material-icons.mat-ligature-font.mat-icon-no-color').should('contain', 'close');
    cy.get('div#cdk-overlay-0.cdk-overlay-pane', { timeout: 6000 }).should('not.exist');
}

function CWECheck() {
    cy.get('input[placeholder="Search..."]').type('dum').then(() => {
        cy.get('input[placeholder="Search..."]').should('have.value', 'dum').and('be.visible');
    });

    //Check the cwe filter
    cy.get('input.checkbox').then(() => {
        cy.get('input.checkbox').should('be.visible').check();

        //Check if filter is checked (shouldnt be)
        cy.get('input.checkbox').eq(5).should('not.be.checked');
    });
}

function ClearFiltersButtonCheck() {
    cy.get('button#clear_button').should('contain', 'Clear').and('be.disabled');
    cy.get('input.checkbox').then(() => {
        //Check filters
        cy.get('input.checkbox').should('be.visible').check();
        cy.get('input.checkbox').should('be.checked');

        //Clear filters
        cy.get('button#clear_button').should('be.enabled').click();
        cy.get('button#clear_button').should('contain', 'Clear').and('be.disabled');
    });
}

function SingleFilterClearButton() {
    cy.get('button#clear_button').should('contain', 'Clear').and('be.disabled');

    //Project Name
    CheckOneFilter(0);

    //Details
    CheckOneFilter(1);

    //Impact
    CheckOneFilter(2);

    //Repeatability
    CheckOneFilter(3);

    //References
    CheckOneFilter(4);

    //CWE
    CheckOneFilter(5);
}

function NoSearchItems() {
    cy.get('input[placeholder = "Search..."]').type('dgurug');
    cy.get('input.checkbox').first().should('be.visible').check();
    cy.get('input.checkbox').should('be.checked');

    cy.intercept('GET', `/api/project-reports/findings?page=1&projectName=dgurug&details=&impact=&repeatability=&references=&cwe=`, {
        statusCode: 404,
        totalpost: 0
    }).as('errorResponse');

    cy.get('button#search_button').click();

    cy.wait('@errorResponse').its('response.statusCode').should('equal', 404)

    cy.on('window:console', (consoleMessage) => {
        expect(consoleMessage).to.have.property('type', 'error');
        expect(consoleMessage).to.have.property('text', 'GET https://localhost:7075/project-reports/findings?page=1&projectName=${badProjectName}&details=&impact=&repeatability=&references=&cwe= 404 (Not Found)');
    });

    Popup('Attention', 'No findings found.', 'info');
}

function DeleteItems() {
    cy.intercept('DELETE', '/api/project-reports', {
        statuscode: 200,
        fixture: 'deletion-response.json'
    }).as('deleteResponse')

    cy.get('mat-icon.mat-icon.notranslate.icon.delete-selected-findings-button.material-icons.mat-ligature-font.mat-icon-no-color')
        .click();

    cy.wait('@deleteResponse').its('response.statusCode').should('equal', 200)

    cy.get('h4.ng-star-inserted').should('contain', 'Found findings: 14');
    cy.get('div.card-body').should('have.length', 16);

    Popup('Attention', 'Successfully removed from db!', 'info');
}