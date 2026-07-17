# ============================================================================
# portfolio-web — Angular 20 SSR (Node runtime)
#   docker build -t portfolio-web .
# ============================================================================

FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:22-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=production PORT=4000
COPY --from=build /app/dist/portfolio-web ./dist/portfolio-web
EXPOSE 4000
CMD ["node", "dist/portfolio-web/server/server.mjs"]
