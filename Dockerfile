FROM registry.nexdev.net:5050/docker/dotnet/core/sdk:6.0 AS builder
# Install the tools
RUN dotnet tool install --tool-path /tools dotnet-trace
RUN dotnet tool install --tool-path /tools dotnet-dump
RUN dotnet tool install --tool-path /tools dotnet-counters

WORKDIR /src

COPY ./.sonarlint/. ./.sonarlint
COPY ./src/OWP/. ./src/OWP
COPY ./src/OWP.OtherSport/. ./src/OWP.OtherSport
COPY ./src/OWP.ApiSyncWorkingShift/. ./src/OWP.ApiSyncWorkingShift
COPY ./src/OWP.PreMatchNotification/. ./src/OWP.PreMatchNotification
COPY ./src/OWP.Maxbet/. ./src/OWP.Maxbet
COPY ./src/OWP.ProductAssignmentReference/. ./src/OWP.ProductAssignmentReference
COPY ./src/OWP.SimpleDataApi/. ./src/OWP.SimpleDataApi
COPY ./src/OWP.TicketTypeRecipient/. ./src/OWP.TicketTypeRecipient
COPY ./src/OWP.RecipientGroup/. ./src/OWP.RecipientGroup
COPY ./src/OWP.TicketCenter/. ./src/OWP.TicketCenter
COPY ./src/OWP.OvertimeReport/. ./src/OWP.OvertimeReport
COPY ./src/OWP.FileUpload/. ./src/OWP.FileUpload
COPY ./src/OWP.AutoAssignment/. ./src/OWP.AutoAssignment
COPY ./src/OWP.PrematchAssignment/. ./src/OWP.PrematchAssignment
COPY ./src/OWP.PreMatchReport/. ./src/OWP.PreMatchReport
COPY ./src/OWP.MatchCountLiveReport/. ./src/OWP.MatchCountLiveReport
COPY ./src/OWP.RedisPubSub/. ./src/OWP.RedisPubSub
COPY ./src/OWP.RabbitMQ/. ./src/OWP.RabbitMQ
COPY ./OWP.Websites/OWP.Websites.csproj ./OWP.Websites/OWP.Websites.csproj

WORKDIR /src/OWP.Websites
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v diag

COPY ./OWP.Websites .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app 

FROM registry.nexdev.net:5050/docker/dotnet/core/aspnet:6.0 AS base

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app
COPY --from=builder /app .
COPY --from=builder /tools /tools

ENV TZ="America/La_Paz" 

ENTRYPOINT ["dotnet", "OWP.Websites.dll"]