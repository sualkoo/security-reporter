# Use the official .NET SDK image as the base image for building the .NET app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dotnet-build
WORKDIR /app

# Copy and restore the .NET app dependencies
COPY webapi/webapi.csproj .
RUN dotnet restore

# Copy the rest of the .NET app code
COPY webapi .
RUN dotnet publish -c Release -o out

# Use the official Node.js image as the base image for building the Angular app
FROM node:18-bullseye AS angular-build
WORKDIR /app

# Copy and install Angular app dependencies
COPY angularapp/package*.json ./
RUN npm install

# Copy the rest of the Angular app code
COPY angularapp .
RUN npm run build

# Use a smaller image for serving the Angular app
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Install texlive-full for generating PDFs
# Uncomment: RUN apt-get update && apt-get install -y texlive-full

# Copy the compiled .NET app from the dotnet-build stage
COPY --from=dotnet-build /app/out /app

# Copy the built Angular app to the .NET app's wwwroot
COPY --from=angular-build /app/dist/angularapp /app/wwwroot

# Expose port for Angular app
EXPOSE 80

# Set the entry point for the container
CMD ["dotnet", "webapi.dll"]
