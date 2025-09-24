# E-Commerce API

## Overview

A clean architecture ASP.NET Core 9.0 Web API for e-commerce, supporting JWT authentication, refresh tokens, product management, image upload, and unit testing.

## Features

- JWT authentication & refresh token endpoint
- Product CRUD with image upload
- Soft delete, Unit of Work, Generic Repository
- Clean separation: API, Application, Domain, Infrastructure
- Unit tests (xUnit)

## Getting Started

1. Clone the repo
2. Run `dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api`
3. Run the API: `dotnet run --project E-Commerce.Api`
4. Access Swagger at `https://localhost:7121/swagger`

## Auth Endpoints

- `/api/Auth/login` — Login, returns JWT & refresh token
- `/api/Auth/refresh` — Send refresh token, returns new JWT & refresh token

## Product Endpoints

- `/api/Product` — CRUD, supports image upload (see IMAGE_UPLOAD_README.md)

## Testing

Run unit tests:

```
dotnet test E-Commerce.Tests
```

## .gitignore

- Ignores build, user, and log files
- Keeps source, configs, and documentation only

## Notes

- All tokens must be sent in `Authorization: Bearer {token}` header

---
