# Project setup
1. Clone repository

2. Download and install Cosmos DB Emulator:
   - https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21

3. Download and install nodeJS: 
   - https://nodejs.org/en/download/current

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
