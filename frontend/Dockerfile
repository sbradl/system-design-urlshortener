FROM node:24-alpine AS builder

WORKDIR /source

COPY --link package.json package-lock.json ./
RUN npm ci

COPY --link . .

RUN npm run test && RUN npm run build

FROM nginx:1.29-alpine
WORKDIR /app
COPY --link --from=builder /source/dist/*/browser /usr/share/nginx/html
