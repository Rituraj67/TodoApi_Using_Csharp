# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the current directory contents into the container's working directory
COPY . .

# Restore any dependencies for the .NET project
RUN dotnet restore

# Publish the .NET project to the /app/out directory
RUN dotnet publish -c Release -o out

# Use the official .NET Runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set the working directory in the container
WORKDIR /app

# Copy the published application from the build environment into the runtime environment
COPY --from=build-env /app/out .

# Expose the port on which the API will be available
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "MyTodo.dll"]