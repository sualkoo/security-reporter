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

# Setting of Safe storage of app secrets in development in ASP.NET Core

TODO: Enable secret storage on your machine and set Primary Key of your cosmos DB emulator.

1. Enable secret storage

   - The Secret Manager tool includes an init command. To use user secrets, run the following command in the project directory:

   ```
   dotnet user-secrets init
   ```

2. Set a secret

   ```
   dotnet user-secrets set "DB:PrimaryKey" "your cosmos db primary key from cosmosDB data explorer"
   ```

3. Create comsos DB Emulator Container and Database with name ProjectDatabase and ProjectContainer. Paste following items.
   - In Database id: ProjectDatabase
   - In Container id: ProjectContainer
   - In Partition Key: id

4. Create comsos DB Emulator Container and Database with name ProjectRolesDatabase and ProjectRolesContainer. Paste following items.
   - In Database id: ProjectRolesDatabase
   - In Container id: ProjectRolesContainer
   - In Partition Key: id

- [ASP.NET Safe storage](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)
