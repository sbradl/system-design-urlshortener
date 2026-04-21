FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder
WORKDIR /source

COPY --link Redirector.slnx .
COPY --link Redirector/Redirector.csproj ./Redirector/Redirector.csproj
COPY --link Redirector.Test/Redirector.Test.csproj ./Redirector.Test/Redirector.Test.csproj
RUN dotnet restore

COPY --link . .
RUN dotnet test --no-restore
RUN dotnet publish Redirector/Redirector.csproj --no-restore -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
EXPOSE 8080
WORKDIR /app
COPY --link --from=builder /app .
USER $APP_UID
ENTRYPOINT [ "./Redirector" ]
