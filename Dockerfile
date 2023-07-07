FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /source

COPY *.sln .
COPY WatsonSync/WatsonSync.csproj ./WatsonSync/
RUN dotnet restore -r linux-musl-x64 /p:PublishReadyToRun=true WatsonSync/WatsonSync.csproj

COPY WatsonSync/. ./WatsonSync/
WORKDIR /source/WatsonSync
RUN dotnet publish -c release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine-amd64
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80

ENTRYPOINT ["./WatsonSync"]
