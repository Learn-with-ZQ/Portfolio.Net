# Personal Professional Platform — Muhammad Zaid Qasim

An enterprise-grade professional digital identity platform: portfolio, résumé, career
dashboard, recruiter portal, blog, testimonial platform, document repository and more —
built on ASP.NET Core 9, Angular 20 (SSR) and SQL Server.

## Solution structure

| Path | What it is | Stack |
|---|---|---|
| `Portfolio/src/` | Clean-architecture backend (Api / Application / Domain / Infrastructure / Persistence) | ASP.NET Core 9, Dapper, Serilog, JWT + refresh tokens |
| `Portfolio/portfolio-web/` | Public website (SSR) | Angular 20, standalone components, signals, Material |
| `Portfolio/portfolio-admin/` | Admin portal (SPA) | Angular 20, Material |
| `Database/` | Schema + stored procedures (`00_Deploy_All.sql` runs everything in order) | SQL Server 2019+ |
| `docs/` | API reference, developer guide, deployment guide | — |

## Feature map

- **Public site** — Home (hero + career story), About, Recruiter page, Experience timeline,
  Projects (search/filter/detail), Skills (radar/matrix/cloud), Education, Awards,
  Certifications, Blog (markdown), Testimonials (public submit → admin approve),
  References (permission-based contact visibility), ATS résumé (print/PDF/export),
  Portfolio-PDF book, Document Center (`/documents`, `/downloads`), data-grounded
  Assistant, Contact. SEO (OG, JSON-LD, sitemap, robots) + accessibility (skip link,
  reduced motion).
- **Admin portal** — CRUD for all 7 core modules + Testimonials approval queue, Blog
  editor with publish toggle, References, Content blocks (paragraphs), Analytics
  dashboard (visitors, downloads, views by country/browser/device).
- **API** — 13 controllers, typed `ApiEnvelope<T>` responses, stored-procedure data
  access with multi-result-set mapping, RBAC (Admin / Public roles), optimistic
  concurrency (`RowVersion`), soft delete + audit columns everywhere.

## Quick start (local)

```bash
# 1. Database — run against SQL Server 2019+ (SQLCMD mode)
sqlcmd -S localhost -i Database/00_Deploy_All.sql

# 2. API  -> http://localhost:5014  (set a real Jwt:SecretKey first)
dotnet run --project Portfolio/src/Portfolio.Api

# 3. Public site -> http://localhost:4200
cd Portfolio/portfolio-web && npm install && npm start

# 4. Admin portal -> http://localhost:4200 (run on another port: ng serve --port 4300)
cd Portfolio/portfolio-admin && npm install && npm start
```

Seeded users: `admin / Admin@123` (full access) · `public / Public@123` (read-only —
used automatically by the public site). **Change both passwords before going live.**

## Docker

```bash
JWT_SECRET=<32+ char secret> docker compose up --build
# api -> :5014, web -> :4000, admin -> :4300, sqlserver -> :1433
# then run Database/00_Deploy_All.sql against the container once
```

## Documents & privacy

Files live on disk, the database stores metadata only (`dbo.Documents`). The API
statically serves **one public-only folder** (`Documents:PhysicalRoot` → `/files`).
Per-document flags: `IsPublic` (served at all) and `IsDownloadable` (download button vs
view-only). **Identity documents (CNIC, passport, domicile) must never be placed in the
served folder.**

## More documentation

- [docs/API.md](docs/API.md) — endpoint reference
- [docs/DEVELOPER.md](docs/DEVELOPER.md) — local setup in depth
- [docs/DEPLOYMENT.md](docs/DEPLOYMENT.md) — Docker & Azure
- [Portfolio/ARCHITECTURE.md](Portfolio/ARCHITECTURE.md) — backend clean-architecture design
