# Use the appropriate SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "MWL.IdentityService.Api.dll"]

# FROM mcr.microsoft.com/dotnet/sdk AS build-env
# WORKDIR /app

# # Copy csproj and restore as distinct layers
# COPY ./MWL.ContentService.Api/*.csproj ./
# RUN dotnet restore

# # Copy everything else and build
# COPY . ./

# # No need to RUN dotnet publish if you're going to use dotnet watch
# # Setting up for development:
# CMD ["dotnet", "watch", "run", "--project", "MWL.ContentService.Api/MWL.ContentService.Api.csproj"]
