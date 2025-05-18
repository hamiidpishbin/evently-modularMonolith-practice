FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["src/API/Evently.Api/Evently.Api.csproj", "src/API/Evently.Api/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Infrastructure/Evently.Modules.Events.Infrastructure.csproj", "src/Modules/Events/Evently.Modules.Events.Infrastructure/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Application/Evently.Modules.Events.Application.csproj", "src/Modules/Events/Evently.Modules.Events.Application/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Domain/Evently.Modules.Events.Domain.csproj", "src/Modules/Events/Evently.Modules.Events.Domain/"]
COPY ["src/Modules/Events/Evently.Modules.Events.Presentation/Evently.Modules.Events.Presentation.csproj", "src/Modules/Events/Evently.Modules.Events.Presentation/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Infrastructure/Evently.Modules.Users.Infrastructure.csproj", "src/Modules/Users/Evently.Modules.Users.Infrastructure/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Application/Evently.Modules.Users.Application.csproj", "src/Modules/Users/Evently.Modules.Users.Application/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Domain/Evently.Modules.Users.Domain.csproj", "src/Modules/Users/Evently.Modules.Users.Domain/"]
COPY ["src/Modules/Users/Evently.Modules.Users.Presentation/Evently.Modules.Users.Presentation.csproj", "src/Modules/Users/Evently.Modules.Users.Presentation/"]
RUN dotnet restore "src/API/Evently.Api/Evently.Api.csproj"
COPY . .
WORKDIR "/src/src/API/Evently.Api"
RUN dotnet build "Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "Evently.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Evently.Api.dll"]