# E-Commerce API 🛍️# E-Commerce API 🛍️



**ASP.NET Core 9.0 Clean Architecture E-Commerce API****ASP.NET Core 9.0 Clean Architecture E-Commerce API**



A production-ready e-commerce API featuring JWT authentication, shopping cart, product management, AutoMapper, FluentValidation, and comprehensive testing.A production-ready e-commerce API featuring JWT authentication, shopping cart, product management, AutoMapper, FluentValidation, and comprehensive testing.



------



## ✨ Features## ✨ Features



### Core Features### Core Features

- ✅ **JWT Authentication** with refresh token support- ✅ **JWT Authentication** with refresh token support

- ✅ **Product Management** with image upload (5MB max, multiple formats)- ✅ **Product Management** with image upload (5MB max, multiple formats)

- ✅ **Shopping Cart** with price snapshots and stock validation- ✅ **Shopping Cart** with price snapshots and stock validation

- ✅ **AutoMapper** for clean entity-DTO mapping- ✅ **AutoMapper** for clean entity-DTO mapping

- ✅ **FluentValidation** for complex validation rules- ✅ **FluentValidation** for complex validation rules

- ✅ **Soft Delete Pattern** across all entities- ✅ **Soft Delete Pattern** across all entities

- ✅ **Unit of Work & Repository Pattern**- ✅ **Unit of Work & Repository Pattern**

- ✅ **Comprehensive Unit Tests** (xUnit + Moq) - 20/20 passing- ✅ **Comprehensive Unit Tests** (xUnit + Moq) - 20/20 passing



### Architecture Highlights### Architecture Highlights

- **Clean Architecture**: Domain, Application, Infrastructure, API layers- **Clean Architecture**: Domain, Application, Infrastructure, API layers

- **Design Patterns**: Repository, Unit of Work, Dependency Injection- **Design Patterns**: Repository, Unit of Work, Dependency Injection

- **Response Pattern**: Consistent `ApiResponse<T>` wrapper- **Response Pattern**: Consistent `ApiResponse<T>` wrapper

- **Exception Handling**: Global middleware with proper HTTP status mapping- **Exception Handling**: Global middleware with proper HTTP status mapping



------



## 🚀 Getting Started## 🚀 Getting Started



### Prerequisites### Prerequisites

- **.NET 9.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/9.0))- **.NET 9.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/9.0))

- **SQL Server** (LocalDB or full instance)- **SQL Server** (LocalDB or full instance)

- **Visual Studio 2022** or **VS Code** with C# extensions- **Visual Studio 2022** or **VS Code** with C# extensions



### Quick Setup### Quick Setup



1. **Clone the repository**1. **Clone the repository**

   ```bash   ```bash

   git clone https://github.com/eslamsalah5/ecomerceTaskDotNet.git   git clone https://github.com/eslamsalah5/ecomerceTaskDotNet.git

   cd E-Commerce   cd E-Commerce

   ```   ```



2. **Update connection string** (if needed)2. **Update connection string** (if needed)

   ```json   ```json

   // appsettings.json   // appsettings.json

   "ConnectionStrings": {   "ConnectionStrings": {

     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true"     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true"

   }   }

   ```   ```



3. **Apply migrations**3. **Apply migrations**

   ```bash   ```bash

   dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api   dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api

   ```   ```



4. **Run the API**4. **Run the API**

   ```bash   ```bash

   dotnet run --project E-Commerce.Api   dotnet run --project E-Commerce.Api

   ```   ```



5. **Access Swagger UI**5. **Access Swagger UI**

   ```   ```

   https://localhost:7121/swagger   https://localhost:7121/swagger

   ```   ```



### Default Seeded Users### Default Seeded Users



| Email | Password | Role || Email | Password | Role |

|-------|----------|------||-------|----------|------|

| `admin@admin.com` | `Admin123` | Admin || `admin@admin.com` | `Admin123` | Admin |

| `customer@test.com` | `Customer123` | Customer || `customer@test.com` | `Customer123` | Customer |



---```



## 📚 API Documentation┌─────────────────────────────────────────┐## 📚 API Documentation



### Authentication Endpoints│           E-Commerce.Api                │  ← Controllers, Middleware

- `POST /api/Auth/login` — Login, returns JWT & refresh token

- `POST /api/Auth/register` — Register new user (Customer role)│           (Presentation)                │### Auth Endpoints

- `POST /api/Auth/refresh` — Refresh access token using refresh token

├─────────────────────────────────────────┤- `POST /api/Auth/login` — Login, returns JWT & refresh token

### Product Endpoints

- `GET /api/Product` — Get all products (paginated, sortable)│       E-Commerce.Application            │  ← Services, DTOs, Interfaces- `POST /api/Auth/register` — Register new user (Customer role)

- `GET /api/Product/{id}` — Get product by ID

- `POST /api/Product` — Create product (Admin only, supports image upload)│       (Business Logic)                  │     Mappings, Validators- `POST /api/Auth/refresh` — Refresh access token using refresh token

- `PUT /api/Product/{id}` — Update product (Admin only)

- `DELETE /api/Product/{id}` — Soft delete product (Admin only)├─────────────────────────────────────────┤



**Sorting Support:**│      E-Commerce.Infrastructure          │  ← EF Core, Repositories### Product Endpoints

```

GET /api/Product?PageNumber=1&PageSize=20&SortBy=Price&SortOrder=desc│      (Data Access)                      │     ImageService, UnitOfWork- `GET /api/Product` — Get all products (paginated)

GET /api/Product?SortBy=Name&SortOrder=asc

```├─────────────────────────────────────────┤- `GET /api/Product/{id}` — Get product by ID



### Cart Endpoints (Requires Auth)│         E-Commerce.Domain               │  ← Entities, Exceptions- `POST /api/Product` — Create product (Admin only, supports image upload)

- `GET /api/Cart` — Get current user's cart

- `POST /api/Cart/items` — Add item to cart│         (Core Domain)                   │     Shared Types- `PUT /api/Product/{id}` — Update product (Admin only)

- `PUT /api/Cart/items/{productId}` — Update item quantity

- `POST /api/Cart/items/{productId}/increment` — Increment quantity by 1└─────────────────────────────────────────┘- `DELETE /api/Product/{id}` — Soft delete product (Admin only)

- `POST /api/Cart/items/{productId}/decrement` — Decrement quantity by 1

- `DELETE /api/Cart/items/{productId}` — Remove item from cart```

- `DELETE /api/Cart` — Clear all items from cart

### Cart Endpoints (Requires Auth)

---

### Key Design Patterns- `GET /api/Cart` — Get current user's cart

## 🏗️ Architecture

- `POST /api/Cart/items` — Add item to cart

### Clean Architecture Layers

1. **Repository Pattern**: Generic repository for common operations- `PUT /api/Cart/items/{productId}` — Update item quantity

```

┌─────────────────────────────────────────┐2. **Unit of Work**: Coordinates multiple repository operations- `POST /api/Cart/items/{productId}/increment` — Increment quantity by 1

│           E-Commerce.Api                │  ← Controllers, Middleware

│           (Presentation)                │3. **Dependency Injection**: All services registered in DI container- `POST /api/Cart/items/{productId}/decrement` — Decrement quantity by 1

├─────────────────────────────────────────┤

│       E-Commerce.Application            │  ← Services, DTOs, Interfaces4. **ApiResponse Wrapper**: Consistent response format across all endpoints- `DELETE /api/Cart/items/{productId}` — Remove item from cart

│       (Business Logic)                  │     Mappings, Validators

├─────────────────────────────────────────┤5. **Soft Delete**: Entities never hard-deleted, only marked `IsDeleted`- `DELETE /api/Cart` — Clear all items from cart

│      E-Commerce.Infrastructure          │  ← EF Core, Repositories

│      (Data Access)                      │     ImageService, UnitOfWork6. **AutoMapper**: Centralized entity-DTO mapping with profiles

├─────────────────────────────────────────┤

│         E-Commerce.Domain               │  ← Entities, Exceptions7. **Global Exception Handling**: Middleware catches and formats all exceptions## 🧪 Testing

│         (Core Domain)                   │     Shared Types

└─────────────────────────────────────────┘---Run all unit tests:

```

```powershell

### Key Design Patterns

1. **Repository Pattern**: Generic repository for common operations## 🚀 Getting Starteddotnet test E-Commerce.Tests

2. **Unit of Work**: Coordinates multiple repository operations

3. **Dependency Injection**: All services registered in DI container```

4. **ApiResponse Wrapper**: Consistent response format across all endpoints

5. **Soft Delete**: Entities never hard-deleted, only marked `IsDeleted`### Prerequisites

6. **AutoMapper**: Centralized entity-DTO mapping with profiles

7. **Global Exception Handling**: Middleware catches and formats all exceptions- **.NET 9.0 SDK** ([Download](https://dotnet.microsoft.com/download/dotnet/9.0))**Current Status**: 20/20 tests passing ✅



---- **SQL Server** (LocalDB or full instance)



## 📁 Project Structure- **Visual Studio 2022** or **VS Code** with C# extensions## 📖 Documentation Files



```### Quick Setup- **PHASE_0_COMPLETE.md** — Summary of Phase 0 improvements

E-Commerce/

├── E-Commerce.Api/                    # Presentation Layer- **AUTOMAPPER_GUIDE.md** — Complete AutoMapper integration guide

│   ├── Controllers/

│   ├── Middleware/1. **Clone the repository**- **VALIDATION_GUIDE.md** — Validation rules quick reference

│   └── Configurations/

├── E-Commerce.Application/            # Business Logic Layer   ````bash- **CART_IMPLEMENTATION.md** — Cart feature implementation details

│   ├── DTOs/

│   ├── Validators/   git clone https://github.com/eslamsalah5/ecomerceTaskDotNet.git- **.github/copilot-instructions.md** — AI agent instructions for the codebase

│   ├── Services/

│   ├── Mappings/   cd E-Commerce

│   └── Interfaces/

├── E-Commerce.Domain/                 # Core Domain Layer   ```## 🎯 Code Quality Standards

│   ├── Entities/

│   ├── Exceptions/   ````

│   └── Shared/

├── E-Commerce.Infrastructure/         # Data Access Layer2. **Update connection string** (if needed)### Validation Rules

│   ├── Repositories/

│   ├── Persistence/   ```json- ✅ All DTOs have detailed validation messages

│   └── Services/

└── E-Commerce.Tests/                  # Unit Tests   // appsettings.json- ✅ `DiscountRate`: 0-100 (percentage)

```

   "ConnectionStrings": {- ✅ `ProductCode`: Format `P01`, `P02`, `P123`, etc.

---

     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true"- ✅ `Quantity`: 1-1000 (realistic limits)

## 🧪 Testing

   }- ✅ MaxLength defined for all string fields

### Run All Tests

```bash   ```

dotnet test E-Commerce.Tests

```### Response Format



### Test Coverage3. **Apply migrations**```json

| Test Suite | Tests | Status |

|------------|-------|--------|   ````bash{

| **AuthServiceTests** | 3 | ✅ Passing |

| **ProductServiceTests** | 3 | ✅ Passing |   dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api  "success": true,

| **CartServiceTests** | 10 | ✅ Passing |

| **CartService_IncrementDecrementTests** | 7 | ✅ Passing |   ```  "statusCode": 200,

| **Total** | **20** | **✅ 100%** |   ````



---"message": "Operation successful",



## ✅ Validation Rules4. **Run the API** "data": { ... },



### Product Validation   ```bash "errors": null

| Field | Rules | Error Message |

|-------|-------|---------------|   dotnet run --project E-Commerce.Api}

| **ProductCode** | Required, Regex: `^P\d{2,}$` | "Must start with 'P' followed by at least 2 digits" |

| **Name** | Required, MaxLength: 200 | "Product name is required" |   ```

| **Price** | Required, Range: 0.01-1000000 | "Price must be between 0.01 and 1,000,000" |

| **Stock** | Required, Range: 0-int.Max | "Stock cannot be negative" |5. **Access Swagger UI**## 🏗️ Project Structure

| **DiscountPercentage** | Optional, Range: 0-100 | "Must be between 0 and 100" |

   ````

### Auth Validation

| Field | Rules | Error Message |   https://localhost:7121/swagger```

|-------|-------|---------------|

| **Email** | Required, EmailAddress | "A valid email address is required" |   ```E-Commerce/

| **Password** | Required, MinLength: 6 | "Password must be at least 6 characters" |   ````



### Cart Validation├── E-Commerce.Api/ # Presentation layer

| Field | Rules | Error Message |

|-------|-------|---------------|### Default Seeded Users│ ├── Controllers/

| **Quantity** | Required, Range: 1-1000 | "Quantity must be between 1 and 1000" |

│ ├── Middleware/

---

| Email | Password | Role |│ └── Configurations/

## 🛠️ Technologies Used

|-------|----------|------|├── E-Commerce.Application/ # Business logic

- **Framework**: ASP.NET Core 9.0

- **ORM**: Entity Framework Core| `admin@admin.com` | `Admin123` | Admin |│ ├── Services/

- **Authentication**: JWT Bearer + Identity

- **Mapping**: AutoMapper| `customer@test.com` | `Customer123` | Customer |│ ├── DTOs/

- **Validation**: Data Annotations + FluentValidation

- **Testing**: xUnit, Moq│ ├── Interfaces/

- **Database**: SQL Server

- **Documentation**: Swagger/OpenAPI### Default Seeded Products│ ├── Mappings/ # AutoMapper profiles



---10 sample products across categories: Electronics, Clothing, Books, Home│ └── Validators/ # FluentValidation



## 🔒 Security Features├── E-Commerce.Domain/ # Core entities



- JWT authentication with 24-hour expiry---│ ├── Entities/

- Refresh tokens with 7-day expiry

- Role-based authorization (Admin, Customer)│ ├── Exceptions/

- Password requirements (min 6 chars, uppercase, lowercase, digit)

- Secure image upload with validation## 📚 API Documentation│ └── Shared/



---├── E-Commerce.Infrastructure/ # Data access



## 📖 Key Concepts### Authentication Endpoints│ ├── Repositories/



### ApiResponse Pattern│ ├── Persistence/

All endpoints return consistent response format:

```json#### POST `/api/Auth/login`│ ├── Services/

{

  "success": true/false,Login with email and password.│ └── Configurations/

  "message": "Operation result message",

  "data": { ... },└── E-Commerce.Tests/ # Unit tests

  "errors": [ ... ],

  "statusCode": 200**Request:**```

}

```````json



### Soft Delete Pattern{## 🛠️ Technologies Used

Entities are never hard-deleted. Instead, they're marked as deleted:

```csharp  "email": "admin@admin.com",

product.IsDeleted = true;

product.DeletedAt = DateTime.UtcNow;  "password": "Admin123"- **Framework**: ASP.NET Core 9.0

```

}- **ORM**: Entity Framework Core

### Price Snapshot in Cart

When items are added to cart, current price and discount are captured to ensure cart total remains consistent even if product prices change later:```- **Authentication**: JWT Bearer + Identity

```csharp

UnitPriceAtAdd = product.Price           // Snapshot- **Mapping**: AutoMapper

DiscountPercentageAtAdd = product.DiscountPercentage  // Snapshot

```**Response:**- **Validation**: Data Annotations + FluentValidation



---```json- **Testing**: xUnit, Moq



## 🛠️ Development Guide{- **Database**: SQL Server



### Common Commands  "success": true,- **Documentation**: Swagger/OpenAPI



```bash  "message": "Login successful",

# Build

dotnet build  "data": {## 🔒 Security Features



# Run    "token": "eyJhbGciOiJIUzI1NiIs...",

dotnet run --project E-Commerce.Api

    "refreshToken": "abc123...",- JWT authentication with 24-hour expiry

# Test

dotnet test E-Commerce.Tests    "email": "admin@admin.com",- Refresh tokens with 7-day expiry



# Create Migration    "firstName": "Admin",- Role-based authorization (Admin, Customer)

dotnet ef migrations add MigrationName --project E-Commerce.Infrastructure --startup-project E-Commerce.Api

    "lastName": "User",- Password requirements (min 6 chars, uppercase, lowercase, digit)

# Update Database

dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api    "roles": ["Admin"],- Secure image upload with validation

```

    "expiresAt": "2025-10-09T10:00:00Z"

### Adding a New Feature

  },## 📝 Notes

1. **Create Entities** in `Domain/Entities/{Feature}/`

2. **Create DTOs** in `Application/DTOs/{Feature}/`  "statusCode": 200

3. **Create Validators** in `Application/Validators/{Feature}/`

4. **Create AutoMapper Profile** in `Application/Mappings/`}- All protected endpoints require `Authorization: Bearer {token}` header

5. **Create Repository Interface & Implementation**

6. **Create Service Interface & Implementation**```- Images are stored in `wwwroot/uploads/` with GUID filenames

7. **Create Controller** in `Api/Controllers/`

8. **Register in DI** container- Soft delete is used for Products and Carts

9. **Create Migration** and update database

10. **Write Tests** in `E-Commerce.Tests/`#### POST `/api/Auth/register`- Price snapshots in cart preserve pricing when product prices change



---Register a new user (automatically assigned Customer role).



## 🔧 Configuration## 🚧 Roadmap



### JWT Settings#### POST `/api/Auth/refresh`

```json

{Refresh access token using refresh token.### Phase 0.5 (In Progress)

  "Jwt": {

    "Key": "your-secret-key-here-min-32-characters",- [ ] BaseAuditableEntity with CreatedAt, UpdatedAt, CreatedBy, UpdatedBy

    "Issuer": "YourApp",

    "Audience": "YourAppUsers"---- [ ] RowVersion for concurrency control

  }

}- [ ] Global Query Filter for soft delete

```

### Product Endpoints- [ ] Response Caching

### Image Upload

- **Max Size**: 5MB- [ ] API Versioning

- **Allowed Formats**: JPG, JPEG, PNG, GIF, BMP, WEBP

- **Storage**: `wwwroot/uploads/products/`#### GET `/api/Product?pageNumber=1&pageSize=10`



---Get paginated list of products.### Phase 1 (Planned)



## 🚧 Roadmap- [ ] Orders & Checkout



### Phase 1 (Planned)**Response:**- [ ] Payment integration (Stripe)

- [ ] Orders & Checkout

- [ ] Payment integration (Stripe)```json- [ ] Order status management

- [ ] Order status management

- [ ] Stock reservation{- [ ] Stock reservation



### Phase 2 (Future)  "success": true,

- [ ] Product categories & variants

- [ ] Product search & filtering  "data": {### Phase 2 (Future)

- [ ] Reviews & ratings

- [ ] Wishlist    "data": [- [ ] Product categories & variants

- [ ] Coupons & promotions

      {- [ ] Product search & filtering

---

        "id": 1,- [ ] Reviews & ratings

## 📝 Notes

        "productCode": "P01",- [ ] Wishlist

- All protected endpoints require `Authorization: Bearer {token}` header

- Images are stored in `wwwroot/uploads/` with GUID filenames        "name": "Gaming Laptop",- [ ] Coupons & promotions

- Soft delete is used for Products and Carts

- Price snapshots in cart preserve pricing when product prices change        "category": "Electronics",



---        "price": 1500.00,## 🤝 Contributing



## 📄 License        "stock": 50,



This project is licensed under the MIT License.        "discountPercentage": 10,Contributions are welcome! Please read the `.github/copilot-instructions.md` for code style and architecture guidelines.



---        "finalPrice": 1350.00,



## 👥 Team        "imageUrl": "/uploads/products/laptop.jpg"## 📄 License



- **Repository**: [github.com/eslamsalah5/ecomerceTaskDotNet](https://github.com/eslamsalah5/ecomerceTaskDotNet)      }



---    ],This project is licensed under the MIT License.



**Last Updated**: October 29, 2025      "currentPage": 1,

**Version**: 2.1 (Phase 1 Complete)  

**Status**: ✅ Production Ready    "totalPages": 1,---

    "pageSize": 10,

    "totalCount": 10**Build Status**: ✅ Success

  }**Tests**: 20/20 Passing ✅

}**Last Updated**: October 8, 2025

````

#### POST `/api/Product` (Admin only)

Create new product with image upload.

#### PUT `/api/Product/{id}` (Admin only)

Update existing product.

#### DELETE `/api/Product/{id}` (Admin only)

Soft delete product.

---

### Cart Endpoints (Requires Authentication)

#### GET `/api/Cart`

Get current user's cart with price snapshots.

#### POST `/api/Cart/items`

Add item to cart (validates stock availability).

#### PUT `/api/Cart/items/{productId}`

Update item quantity.

#### POST `/api/Cart/items/{productId}/increment`

Increment item quantity by 1.

#### POST `/api/Cart/items/{productId}/decrement`

Decrement item quantity by 1.

#### DELETE `/api/Cart/items/{productId}`

Remove specific item from cart.

#### DELETE `/api/Cart`

Clear all items from cart.

---

## 📁 Project Structure

### Organized by Features & Domains

```
E-Commerce/
├── E-Commerce.Api/                    # Presentation Layer
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ProductController.cs
│   │   └── CartController.cs
│   ├── Middleware/
│   │   └── GlobalExceptionMiddleware.cs
│   └── Configurations/
│       ├── CorsExtensions.cs
│       ├── JwtExtensions.cs
│       └── SwaggerExtensions.cs
│
├── E-Commerce.Application/            # Business Logic Layer
│   ├── DTOs/
│   │   ├── Auth/                     # Authentication DTOs
│   │   ├── Products/                 # Product DTOs
│   │   └── Cart/                     # Cart DTOs
│   ├── Validators/
│   │   ├── Auth/
│   │   ├── Products/
│   │   └── Cart/
│   ├── Services/
│   ├── Mappings/
│   └── Interfaces/
│
├── E-Commerce.Domain/                 # Core Domain Layer
│   ├── Entities/
│   │   ├── Base/                     # Base classes
│   │   ├── Identity/                 # User management
│   │   ├── Catalog/                  # Products
│   │   └── Shopping/                 # Cart
│   ├── Exceptions/
│   └── Shared/
│
├── E-Commerce.Infrastructure/         # Data Access Layer
│   ├── Repositories/
│   ├── Persistence/
│   ├── Services/
│   └── Data/
│
└── E-Commerce.Tests/                  # Unit Tests
    ├── AuthServiceTests.cs
    ├── ProductServiceTests.cs
    ├── CartServiceTests.cs
    └── CartService_IncrementDecrementTests.cs
```

---

## 🧪 Testing

### Run All Tests

```bash
dotnet test E-Commerce.Tests
```

### Test Coverage

| Test Suite                              | Tests  | Status      |
| --------------------------------------- | ------ | ----------- |
| **AuthServiceTests**                    | 3      | ✅ Passing  |
| **ProductServiceTests**                 | 3      | ✅ Passing  |
| **CartServiceTests**                    | 10     | ✅ Passing  |
| **CartService_IncrementDecrementTests** | 7      | ✅ Passing  |
| **Total**                               | **20** | **✅ 100%** |

---

## 🗺️ AutoMapper Integration

### What is AutoMapper?

AutoMapper eliminates repetitive manual mapping code between entities and DTOs.

### Example Usage

**Before (Manual Mapping):**

```csharp
var productDto = new ProductDto
{
    Id = product.Id,
    Name = product.Name,
    Price = product.Price,
    // ... 10+ more properties
};
```

**After (AutoMapper):**

```csharp
var productDto = _mapper.Map<ProductDto>(product);
```

### Mapping Profiles

#### ProductMappingProfile

```csharp
CreateMap<Product, ProductDto>()
    .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
        !string.IsNullOrEmpty(src.ImagePath) ? $"/{src.ImagePath}" : null))
    .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src =>
        src.Price * (1 - (src.DiscountPercentage ?? 0) / 100m)));
```

### Benefits

- ✅ **Less Code**: Eliminates ~30 lines per service
- ✅ **Centralized Logic**: Mapping rules in one place
- ✅ **Type Safety**: Compile-time checks
- ✅ **Maintainable**: Easy to update mapping rules

---

## ✅ Validation Rules

### Product Validation

| Field                  | Rules                         | Error Message                                       |
| ---------------------- | ----------------------------- | --------------------------------------------------- |
| **ProductCode**        | Required, Regex: `^P\d{2,}$`  | "Must start with 'P' followed by at least 2 digits" |
| **Name**               | Required, MaxLength: 200      | "Product name is required"                          |
| **Price**              | Required, Range: 0.01-1000000 | "Price must be between 0.01 and 1,000,000"          |
| **Stock**              | Required, Range: 0-int.Max    | "Stock cannot be negative"                          |
| **DiscountPercentage** | Optional, Range: 0-100        | "Must be between 0 and 100"                         |

### Auth Validation

| Field        | Rules                  | Error Message                            |
| ------------ | ---------------------- | ---------------------------------------- |
| **Email**    | Required, EmailAddress | "A valid email address is required"      |
| **Password** | Required, MinLength: 6 | "Password must be at least 6 characters" |

### Cart Validation

| Field        | Rules                   | Error Message                         |
| ------------ | ----------------------- | ------------------------------------- |
| **Quantity** | Required, Range: 1-1000 | "Quantity must be between 1 and 1000" |

---

## 🛠️ Development Guide

### Adding a New Feature (Example: Orders)

1. **Create Entities** in `Domain/Entities/Orders/`
2. **Create DTOs** in `Application/DTOs/Orders/`
3. **Create Validators** in `Application/Validators/Orders/`
4. **Create AutoMapper Profile** in `Application/Mappings/`
5. **Create Repository Interface & Implementation**
6. **Create Service Interface & Implementation**
7. **Create Controller** in `Api/Controllers/`
8. **Register in DI** container
9. **Create Migration** and update database
10. **Write Tests** in `E-Commerce.Tests/`

### Common Commands

```bash
# Build
dotnet build

# Run
dotnet run --project E-Commerce.Api

# Test
dotnet test E-Commerce.Tests

# Create Migration
dotnet ef migrations add MigrationName --project E-Commerce.Infrastructure --startup-project E-Commerce.Api

# Update Database
dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.Api
```

---

## 📖 Key Concepts

### ApiResponse Pattern

All endpoints return consistent response format:

```csharp
{
  "success": true/false,
  "message": "Operation result message",
  "data": { ... },
  "errors": [ ... ],
  "statusCode": 200
}
```

### Soft Delete Pattern

Entities are never hard-deleted. Instead, they're marked as deleted:

```csharp
product.IsDeleted = true;
product.DeletedAt = DateTime.UtcNow;
```

Generic repository auto-filters deleted entities in queries.

### Price Snapshot in Cart

When items are added to cart, current price and discount are captured to ensure cart total remains consistent even if product prices change later:

```csharp
UnitPriceAtAdd = product.Price           // Snapshot
DiscountPercentageAtAdd = product.DiscountPercentage  // Snapshot
```

---

## 🔧 Configuration

### JWT Settings

```json
{
  "Jwt": {
    "Key": "your-secret-key-here-min-32-characters",
    "Issuer": "YourApp",
    "Audience": "YourAppUsers"
  }
}
```

### CORS Settings

- **Development**: Allow all origins
- **Production**: Configured via `appsettings.json`

### Image Upload

- **Max Size**: 5MB
- **Allowed Formats**: JPG, JPEG, PNG, GIF, BMP, WEBP
- **Storage**: `wwwroot/uploads/products/`

---

## 📝 License

This project is licensed under the MIT License.

---

## 👥 Team

- **Repository**: [github.com/eslamsalah5/ecomerceTaskDotNet](https://github.com/eslamsalah5/ecomerceTaskDotNet)

---

**Last Updated**: October 8, 2025  
**Version**: 2.0 (Phase 0 Complete)  
**Status**: ✅ Production Ready
