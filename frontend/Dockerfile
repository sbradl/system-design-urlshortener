FROM node:24-alpine AS builder

WORKDIR /source

RUN npm install -g @angular/cli@21

COPY --link package.json package-lock.json ./
RUN npm ci

COPY . .

RUN npm run build


FROM nginx:1.29-alpine
WORKDIR /app
COPY --link --from=builder /source/dist/*/browser /usr/share/nginx/html
