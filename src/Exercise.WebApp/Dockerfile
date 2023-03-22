#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Exercise.WebApp/Exercise.WebApp.csproj", "Exercise.WebApp/"]
COPY ["Exercise.Model/Exercise.Model.csproj", "Exercise.Model/"]
RUN dotnet restore "Exercise.WebApp/Exercise.WebApp.csproj"
COPY . .
WORKDIR "/src/Exercise.WebApp"
RUN dotnet build "Exercise.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exercise.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exercise.WebApp.dll"]