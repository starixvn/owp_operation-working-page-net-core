FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
WORKDIR /home

COPY ./.sonarlint/. ./.sonarlint
COPY ./src/OWP/. ./src/OWP
COPY ./src/OWP.OtherSport/. ./src/OWP.OtherSport
COPY ./src/OWP.Maxbet/. ./src/OWP.Maxbet
COPY ./OWP.Websites/. ./OWP.Websites

WORKDIR /home/OWP.Websites
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v diag

COPY ./OWP.Websites .

RUN dotnet publish -r linux-x64 --self-contained false -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

WORKDIR /app
COPY --from=builder /app .

RUN    apt-get update \
    && apt-get install openssl \
    && apt-get install ca-certificates

RUN sed -i "s|TLSv1.2|TLSv1.0|g" /etc/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "OWP.Websites.dll"]