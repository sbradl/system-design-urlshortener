FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder
WORKDIR /source

COPY --link Shortener.slnx .
COPY --link Shortener/Shortener.csproj ./Shortener/Shortener.csproj
COPY --link Shortener.Test/Shortener.Test.csproj ./Shortener.Test/Shortener.Test.csproj
RUN dotnet restore

COPY --link . .
RUN dotnet test --no-restore
RUN dotnet publish Shortener/Shortener.csproj --no-restore -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --link --from=builder /app .
USER $APP_UID
ENTRYPOINT [ "./Shortener" ]
