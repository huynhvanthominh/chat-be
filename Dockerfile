# Use the official .NET SDK image to build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0.204 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for the base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0.4 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the build stage to the runtime stage
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 80

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "chat_be.dll"]

