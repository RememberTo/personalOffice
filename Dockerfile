FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PersonalOffice.Backend.API/PersonalOffice.Backend.API.csproj", "PersonalOffice.Backend.API/"]
COPY ["message-bus-core/MessageBusCore.csproj", "message-bus-core/"]
COPY ["PersonalOffice.Backend.Application/PersonalOffice.Backend.Application.csproj", "PersonalOffice.Backend.Application/"]
COPY ["PersonalOffice.Backend.Domain/PersonalOffice.Backend.Domain.csproj", "PersonalOffice.Backend.Domain/"]
RUN dotnet restore "./PersonalOffice.Backend.API/./PersonalOffice.Backend.API.csproj"
COPY . .
WORKDIR "/src/PersonalOffice.Backend.API"
RUN dotnet build "./PersonalOffice.Backend.API.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PersonalOffice.Backend.API.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
#COPY --from=publish /app/publish .
COPY --chown=app:app --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalOffice.Backend.API.dll"]