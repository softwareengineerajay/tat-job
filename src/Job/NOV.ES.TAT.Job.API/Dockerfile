#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "Job/"]
COPY ["Job/NOV.ES.TAT.Job.API/NOV.ES.TAT.Job.API.csproj", "Job/NOV.ES.TAT.Job.API/"]
COPY ["Job/NOV.ES.TAT.Job.DomainService/NOV.ES.TAT.Job.DomainService.csproj", "Job/NOV.ES.TAT.Job.DomainService/"]
COPY ["Job/NOV.ES.TAT.Job.Domain/NOV.ES.TAT.Job.Domain.csproj", "Job/NOV.ES.TAT.Job.Domain/"]
COPY ["Job/NOV.ES.TAT.Job.Infrastructure/NOV.ES.TAT.Job.Infrastructure.csproj", "Job/NOV.ES.TAT.Job.Infrastructure/"]
RUN dotnet restore "Job/NOV.ES.TAT.Job.API/NOV.ES.TAT.Job.API.csproj"
COPY . .
WORKDIR "/src/Job/NOV.ES.TAT.Job.API"
RUN dotnet build "NOV.ES.TAT.Job.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NOV.ES.TAT.Job.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NOV.ES.TAT.Job.API.dll"]
