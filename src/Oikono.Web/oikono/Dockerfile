# Build Stage
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Production Stage
FROM node:20-alpine
WORKDIR /app

# serve installieren, um statische Dateien zu hosten
RUN npm install -g serve

# Build kopieren
COPY --from=build /app/dist /app/dist

# Container starten
CMD ["serve", "-s", "dist", "-l", "3000"]
