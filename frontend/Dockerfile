FROM node:24-alpine AS builder

WORKDIR /source

COPY --link package.json package-lock.json ./
RUN npm ci

COPY --link . .

RUN npm run build


# 2. ASP.NET build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-build

WORKDIR /app

COPY --link FrontendHost.csproj Program.cs ./

RUN dotnet restore FrontendHost.csproj
RUN dotnet publish FrontendHost.csproj -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime

WORKDIR /app

# Copy Angular build output
COPY --link --from=dotnet-build /out ./
COPY --link --from=builder /source/dist/frontend/browser ./wwwroot

RUN ls -la /app/wwwroot

# ASP.NET static hosting configuration
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

# Minimal static file server (Kestrel)
ENTRYPOINT ["dotnet", "FrontendHost.dll"]
