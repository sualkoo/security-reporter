# Introduction

TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project.

# Getting Started

TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:

1. Installation process
2. Software dependencies
3. Latest releases
4. API references

# Build and Test

TODO: Describe and show how to build your code and run the tests.

# Contribute

TODO: Explain how other users and developers can contribute to make your code better.

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:

- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)

# Project setup
1. Clone repository

2. Download and install Cosmos DB Emulator:
   - https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21

3. Open Cosmos DB Emulator (emulator should show up in browser)

4. Create comsos DB Emulator Container and Database in explorer tab (menu) with name ProjectDatabase and ProjectContainer. Paste following items.
   - In Database id: ProjectDatabase
   - In Container id: ProjectContainer
   - In Partition Key: id

5. Add to cosmos DB Emulator ProjectDatabase new container:
   - In Container id: ProjectRolesContainer
   - In Partition Key: id

   - For now you need to add users with roles manually in this format:
      - Example: 
      - {
            "id": "client@client.sk",
            "Role": "client"
         }
      - Attribute Role can be admin/client/coordinator/pentester/default.
      - You can add new item by clicking on ProjectRolesDatabase -> ProjectRolesContainer -> Items, then click on New item and then click save.

6. Enable secret storage and set a secret

   - The Secret Manager tool includes an init command. To use user secrets, run the following command in the project directory:
   - You can find your PrimaryKey in Quickstart tab in Emulator.

   ```
   dotnet user-secrets init
   ```

   ```
   dotnet user-secrets set "DB:PrimaryKey" "your cosmos db primary key from cosmosDB data explorer"
   ```

7. Open repository folder in terminal and type:
   ```
   npm install
   ```

8. Open project solution in Visual Studio

9. 
   - In View tab click on Solution explorer.
   - In Solution explorer right click on Solution and choose Properties
   - In menu select Common Properties -> Startup Project
   - Click on Multiple startup project radio button
   - In table set webapi Project will be on top (select it and move it with arrows on right) and select Start Action in right column
   - In table set angularapp Project to second position and select Start Action in right column
   - Click Apply and OK

10. Start project (F5)

11. Swagger and angular page should be visible in browser.

- [ASP.NET Safe storage](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)
