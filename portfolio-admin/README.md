# Portfolio Admin Portal

Angular 20 admin portal for the Portfolio Platform API.

## Stack

- Angular 20 (standalone components, signals)
- Angular Material
- Reactive forms + HttpClient

## Prerequisites

- Node.js 20+
- Portfolio API running at `http://localhost:5014`
- Auth SQL scripts deployed (`12_Portfolio_Auth_Schema.sql`, `13_Portfolio_Auth_Procedures.sql`)

## Run

```bash
npm install
npm start
```

Open `http://localhost:4200`

**Default login:** `admin` / `Admin@123`

## Modules

| Route | Module |
|-------|--------|
| `/dashboard` | Overview |
| `/experience` | Experience CRUD |
| `/projects` | Projects CRUD |
| `/education` | Education CRUD |
| `/skills` | Skills CRUD |
| `/awards` | Awards CRUD |
| `/certifications` | Certifications CRUD |
| `/documents` | Documents CRUD |

## Configuration

API URL: `src/environments/environment.development.ts`

```typescript
apiUrl: 'http://localhost:5014'
```

Use the toolbar **Profile ID** field to switch the active portfolio profile (default: `1`).
