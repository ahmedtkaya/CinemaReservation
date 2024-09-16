# Base image (use .NET SDK to build the app)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy all other files and build the app
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime image (use .NET runtime to run the app)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set working directory inside the container for runtime
WORKDIR /app

# Copy the built app from the build stage
COPY --from=build /app/out .

# Expose the port that the app will listen on
EXPOSE 80

# Set entrypoint for the app
ENTRYPOINT ["dotnet", "cinemaReservation.dll"]
