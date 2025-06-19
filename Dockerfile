FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

COPY ./src/OWP.AccountSetting/OWP.AccountSetting.csproj ./src/OWP.AccountSetting/
COPY ./src/OWP.Api/OWP.Api.csproj ./src/OWP.Api/
COPY ./src/OWP.Application/OWP.Application.csproj ./src/OWP.Application/
COPY ./src/OWP.AssigneeSuggestionList/OWP.AssigneeSuggestionList.csproj ./src/OWP.AssigneeSuggestionList/
COPY ./src/OWP.AssignmentUpload/OWP.AssignmentUpload.csproj ./src/OWP.AssignmentUpload/
COPY ./src/OWP.BackgroundWorker/OWP.BackgroundWorker.csproj ./src/OWP.BackgroundWorker/
COPY ./src/OWP.Event/OWP.Event.csproj ./src/OWP.Event/
COPY ./src/OWP.EventPicking/OWP.EventPicking.csproj ./src/OWP.EventPicking/
COPY ./src/OWP.FileUpload/OWP.FileUpload.csproj ./src/OWP.FileUpload/
COPY ./src/OWP.Kpi/OWP.Kpi.csproj ./src/OWP.Kpi/
COPY ./src/OWP.LeagueManagement/OWP.LeagueManagement.csproj ./src/OWP.LeagueManagement/
COPY ./src/OWP.LiveDetail/OWP.LiveDetail.csproj ./src/OWP.LiveDetail/
COPY ./src/OWP.MaxbetSetting/OWP.MaxbetSetting.csproj ./src/OWP.MaxbetSetting/
COPY ./src/OWP.MessageManagement/OWP.MessageManagement.csproj ./src/OWP.MessageManagement/
COPY ./src/OWP.NewLeagueSetting/OWP.NewLeagueSetting.csproj ./src/OWP.NewLeagueSetting/
COPY ./src/OWP.NotificationSetting/OWP.NotificationSetting.csproj ./src/OWP.NotificationSetting/
COPY ./src/OWP.OddsCompiler/OWP.OddsCompiler.csproj ./src/OWP.OddsCompiler/
COPY ./src/OWP.OSLeagueSetting/OWP.OSLeagueSetting.csproj ./src/OWP.OSLeagueSetting/
COPY ./src/OWP.OtherSport/OWP.OtherSport.csproj ./src/OWP.OtherSport/
COPY ./src/OWP.PrematchAssignment/OWP.PrematchAssignment.csproj ./src/OWP.PrematchAssignment/
COPY ./src/OWP.ProductAssignmentReference/OWP.ProductAssignmentReference.csproj ./src/OWP.ProductAssignmentReference/
COPY ./src/OWP.ProviderArrangement/OWP.ProviderArrangement.csproj ./src/OWP.ProviderArrangement/
COPY ./src/OWP.ProviderVerification/OWP.ProviderVerification.csproj ./src/OWP.ProviderVerification/
COPY ./src/OWP.RabbitMQ/OWP.RabbitMQ.csproj ./src/OWP.RabbitMQ/
COPY ./src/OWP.Report/OWP.Report.csproj ./src/OWP.Report/
COPY ./src/OWP.ResourceManagement/OWP.ResourceManagement.csproj ./src/OWP.ResourceManagement/
COPY ./src/OWP.SimpleDataApi/OWP.SimpleDataApi.csproj ./src/OWP.SimpleDataApi/
COPY ./src/OWP.TicketCenter/OWP.TicketCenter.csproj ./src/OWP.TicketCenter/
COPY ./src/OWP.TicketSetting/OWP.TicketSetting.csproj ./src/OWP.TicketSetting/
COPY ./src/OWP.UserManagement/OWP.UserManagement.csproj ./src/OWP.UserManagement/
COPY ./src/OWP.WorkingShift/OWP.WorkingShift.csproj ./src/OWP.WorkingShift/
COPY ./src/OWP.ChatServer/OWP.ChatServer.csproj ./src/OWP.ChatServer/
COPY ./src/OWP/OWP.csproj ./src/OWP/
COPY ./OWP.Websites/OWP.Websites.csproj ./OWP.Websites/

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n -r linux-x64 ./OWP.Websites/OWP.Websites.csproj

COPY ./.sonarlint/. ./.sonarlint
COPY ./src/. ./src/
COPY ./OWP.Websites ./OWP.Websites

WORKDIR /src/OWP.Websites
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz" 
ENV COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "OWP.Websites.dll"]