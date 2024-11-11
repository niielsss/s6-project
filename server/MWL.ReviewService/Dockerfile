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

ENTRYPOINT ["dotnet", "MWL.ReviewService.Api.dll"]