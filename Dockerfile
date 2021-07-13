FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder
WORKDIR /src

COPY ./.sonarlint/. ./.sonarlint
COPY ./src/OWP/. ./src/OWP
COPY ./src/OWP.OtherSport/. ./src/OWP.OtherSport
COPY ./src/OWP.ApiSyncWorkingShift/. ./src/OWP.ApiSyncWorkingShift
COPY ./src/OWP.PreMatchNotification/. ./src/OWP.PreMatchNotification
COPY ./src/OWP.Maxbet/. ./src/OWP.Maxbet
COPY ./src/OWP.ProductAssignmentReference/. ./src/OWP.ProductAssignmentReference
COPY ./OWP.Websites/OWP.Websites.csproj ./OWP.Websites/OWP.Websites.csproj

WORKDIR /src/OWP.Websites
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v diag

COPY ./OWP.Websites .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:5.0-buster-slim AS base
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz" 

ENTRYPOINT ["dotnet", "OWP.Websites.dll"]