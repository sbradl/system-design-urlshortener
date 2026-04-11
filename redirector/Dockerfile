FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder
WORKDIR /source

COPY --link *.csproj .
RUN dotnet restore

COPY --link . .
RUN dotnet publish --no-restore -o /app


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --link --from=builder /app .
USER $APP_UID
ENTRYPOINT [ "./redirector" ]
