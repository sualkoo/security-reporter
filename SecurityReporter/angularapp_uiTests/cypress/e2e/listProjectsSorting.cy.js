describe("Firestarter", () => {
  const sortAndVerify = (column, sortDirection) => {
    cy.get(`th[mat-header-cell][mat-sort-header="${column}"]`).click();

    cy.wait("@retrieve");

    cy.get("table").as("table");

    let projectColumns = [];

    column = column.replace("Date", "");
    // Get all the cells in the projectName column after sorting and store in the array
    cy.get("@table")
      .find(`.mat-column-${column}`)
      .should("have.length.greaterThan", 1);

    cy.get("@table")
      .find(`.mat-column-${column}`)
      .each((cell) => {
        cy.wrap(cell)
          .invoke("text")
          .then((text) => {
            projectColumns.push(text.trim());
          });
      })
      .then(() => {
        projectColumns.shift();

        // Create a copied and sorted array for comparison
        const copiedprojectColumns = [...projectColumns];

        if (column == "pentestDuration") {
          if (sortDirection === "asc") {
            copiedprojectColumns
              .slice()
              .sort((a, b) => parseInt(a, 10) - parseInt(b, 10));
          } else if (sortDirection === "desc") {
            copiedprojectColumns
              .slice()
              .sort((a, b) => parseInt(b, 10) - parseInt(a, 10));
          }
        } else {
          if (sortDirection === "asc") {
            copiedprojectColumns.sort((a, b) =>
              a.toLowerCase().localeCompare(b.toLowerCase())
            );
          } else if (sortDirection === "desc") {
            copiedprojectColumns.sort((a, b) =>
              b.toLowerCase().localeCompare(a.toLowerCase())
            );
          }
        }

        console.log("sorted", projectColumns);
        console.log("sorted", copiedprojectColumns);

        expect(projectColumns).to.deep.equal(copiedprojectColumns);
      });
  };

  const newTable = (column) => {
    cy.get("table").as("table");

    let projectColumns = [];

    column = column.replace("Date", "");
    // Get all the cells in the projectName column after sorting and store in the array

    cy.get("@table")
      .find(`.mat-column-${column}`)
      .should("have.length.greaterThan", 1);

    cy.get("@table")
      .find(`.mat-column-${column}`)
      .each((cell) => {
        cy.wrap(cell)
          .invoke("text")
          .then((text) => {
            projectColumns.push(text.trim());
          });
      })
      .then(() => {
        console.log(projectColumns);
      });
  };

  const getDefaultColumn = (column) => {
    newTable(column);

    cy.get("table").as("table");

    let oldColumns = [];
    let oldColumnName = column;
    column = column.replace("Date", "");

    // Get all the cells in the projectName column after sorting and store in the array
    cy.get("@table")
      .find(`.mat-column-${column}`)
      .should("have.length.greaterThan", 1);

    cy.get("@table")
      .find(`.mat-column-${column}`)
      .each((cell) => {
        cy.wrap(cell)
          .invoke("text")
          .then((text) => {
            oldColumns.push(text.trim());
          });
      });

    sortAndVerify(oldColumnName, "desc");

    cy.get(`div.reset-filters`).click();

    cy.wait("@retrieve");

    cy.wait("@retrieve").then(() => {
      cy.get("table").as("newtable");

      let arrayOfCollumns = [];

      cy.get("@table")
        .find(`.mat-column-${column}`)
        .should("have.length.greaterThan", 1);

      // Get all the cells in the projectName column after sorting and store in the array
      cy.get("@newtable")
        .find(`.mat-column-${column}`)
        .each((cell) => {
          cy.wrap(cell)
            .invoke("text")
            .then((text) => {
              arrayOfCollumns.push(text.trim());
            });
        })
        .then(() => {
          console.log(oldColumns);

          console.log(arrayOfCollumns);
          expect(arrayOfCollumns).to.deep.equal(oldColumns);
        });
    });
  };

  beforeEach(() => {
    cy.intercept("GET", "**/retrieve*").as("retrieve");

    cy.visit("https://localhost:4200/list-projects");
  });

  it("has a title", () => {
    cy.contains("Project Management");
  });

  it("Reset sorting button", () => {
    cy.wait("@retrieve");

    getDefaultColumn("projectName");
    getDefaultColumn("startDate");
    getDefaultColumn("endDate");
    getDefaultColumn("reportDueDate");
    getDefaultColumn("pentestDuration");
    getDefaultColumn("iko");
    getDefaultColumn("tko");
  });

  it("Project name column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("projectName", "desc");
    sortAndVerify("projectName", "asc");
    sortAndVerify("projectName", "desc");
  });

  it("Start date column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("startDate", "desc");
    sortAndVerify("startDate", "asc");
    sortAndVerify("startDate", "desc");
  });

  it("End date column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("endDate", "desc");
    sortAndVerify("endDate", "asc");
    sortAndVerify("endDate", "desc");
  });

  it("Report Due Date column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("reportDueDate", "desc");
    sortAndVerify("reportDueDate", "asc");
    sortAndVerify("reportDueDate", "desc");
  });

  it("Pentest Duration column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("pentestDuration", "desc");
    sortAndVerify("pentestDuration", "asc");
    sortAndVerify("pentestDuration", "desc");
  });

  it("IKO column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("iko", "desc");
    sortAndVerify("iko", "asc");
    sortAndVerify("iko", "desc");
  });

  it("TKO column 3 clicks (DESC -> ASC -> DESC)", () => {
    cy.wait("@retrieve");

    sortAndVerify("tko", "desc");
    sortAndVerify("tko", "asc");
    sortAndVerify("tko", "desc");
  });
});
