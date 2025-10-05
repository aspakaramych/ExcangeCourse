FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExchangeCourse/ExchangeCourse.csproj", "ExchangeCourse/"]
RUN dotnet restore "ExchangeCourse/ExchangeCourse.csproj"
COPY . .
WORKDIR "/src/ExchangeCourse"
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations add InitialCreate --project ExchangeCourse.csproj --output-dir Migrations --context ApplicationDbContext || echo "Миграция уже существует — пропускаем"
RUN dotnet build "ExchangeCourse.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeCourse.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeCourse.dll"]
