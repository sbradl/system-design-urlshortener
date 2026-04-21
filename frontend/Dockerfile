FROM node:24-alpine AS builder

WORKDIR /source

COPY --link package.json package-lock.json ./
RUN npm ci

COPY --link . .

RUN npm run build
RUN npm run test

FROM nginx:1.29-alpine
WORKDIR /app
COPY --link --from=builder /source/dist/*/browser /usr/share/nginx/html
