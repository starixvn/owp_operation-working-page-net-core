# Prepare Project/*.csproj

FROM hub.nexdev.net/base/dotnet/core/sdk:7.0-alpine AS prepare-restore-files

ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2

WORKDIR /src
COPY . .
RUN dotnet subset restore ./OWP.Websites/OWP.Websites.csproj --root-directory /src --output /src/restore_subset/

FROM hub.nexdev.net/base/dotnet/core/sdk:7.0-alpine AS builder


WORKDIR /src
COPY --from=prepare-restore-files /src/restore_subset .
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore ./OWP.Websites/OWP.Websites.csproj

COPY ./.sonarlint/. ./.sonarlint
COPY ./src/OWP/. ./src/OWP
COPY ./src/OWP.OtherSport/. ./src/OWP.OtherSport
COPY ./src/OWP.ApiSyncWorkingShift/. ./src/OWP.ApiSyncWorkingShift
COPY ./src/OWP.Maxbet/. ./src/OWP.Maxbet
COPY ./src/OWP.MaxbetSetting/. ./src/OWP.MaxbetSetting
COPY ./src/OWP.ProductAssignmentReference/. ./src/OWP.ProductAssignmentReference
COPY ./src/OWP.SimpleDataApi/. ./src/OWP.SimpleDataApi
COPY ./src/OWP.TicketTypeRecipient/. ./src/OWP.TicketTypeRecipient
COPY ./src/OWP.RecipientGroup/. ./src/OWP.RecipientGroup
COPY ./src/OWP.TicketCenter/. ./src/OWP.TicketCenter
COPY ./src/OWP.FileUpload/. ./src/OWP.FileUpload
COPY ./src/OWP.AutoAssignment/. ./src/OWP.AutoAssignment
COPY ./src/OWP.PrematchAssignment/. ./src/OWP.PrematchAssignment
COPY ./src/OWP.MatchCountLiveReport/. ./src/OWP.MatchCountLiveReport
COPY ./src/OWP.OSLeagueSetting/. ./src/OWP.OSLeagueSetting
COPY ./src/OWP.RabbitMQ/. ./src/OWP.RabbitMQ
COPY ./src/OWP.Report/. ./src/OWP.Report
COPY ./src/OWP.LeagueManagement/. ./src/OWP.LeagueManagement
COPY ./src/OWP.LiveDetail/. ./src/OWP.LiveDetail
COPY ./src/OWP.TicketEvent/. ./src/OWP.TicketEvent
COPY ./src/OWP.AssignmentUpload/. ./src/OWP.AssignmentUpload
COPY ./src/OWP.EventPicking/. ./src/OWP.EventPicking
COPY ./src/OWP.ProviderArrangement/. ./src/OWP.ProviderArrangement
COPY ./src/OWP.ResourceManagement/. ./src/OWP.ResourceManagement
COPY ./src/OWP.AssigneeSuggestionList/. ./src/OWP.AssigneeSuggestionList
COPY ./src/OWP.Event/. ./src/OWP.Event
COPY ./src/OWP.Kpi/. ./src/OWP.Kpi
COPY ./OWP.Websites ./OWP.Websites

WORKDIR /src/OWP.Websites
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c release -o /app --no-restore

FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz" 
ENV COMPlus_EnableDiagnostics=0

# Don’t run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "OWP.Websites.dll"]