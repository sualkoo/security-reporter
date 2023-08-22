const { defineConfig } = require("cypress");
const path = require("path");

module.exports = defineConfig({
    fileServerFolder: path.resolve(__dirname, "../../angularapp/src"),
   configFile: path.resolve(__dirname, "../../angularapp/angular.json"),
  e2e: {
    setupNodeEvents(on, config) {
      // implement node event listeners here
      },
    },

    component: {
        devServer: {
            framework: "angular",
            bundler: "webpack",
        },

        specPattern: "**/*.cy.ts",
    }, 
});
