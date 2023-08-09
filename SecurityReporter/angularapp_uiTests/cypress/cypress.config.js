const cypress = require("cypress");
const defineConfig = cypress.defineConfig;

module.exports = defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});
