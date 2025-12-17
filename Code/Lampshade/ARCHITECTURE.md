# معماری پروژه Lampshade

## خلاصه
این پروژه یک فروشگاه آنلاین با معماری **Clean Architecture** و **Domain-Driven Design (DDD)** است که با ASP.NET Core 8.0 و Entity Framework Core با دیتابیس SQLite پیاده‌سازی شده.

---

## ساختار لایه‌ها

```
┌─────────────────────────────────────────────────────────────┐
│                      ServiceHost                             │
│              (Presentation Layer - Razor Pages)              │
├─────────────────────────────────────────────────────────────┤
│                    Configuration Layer                       │
│         (Bootstrappers - Dependency Injection)               │
├─────────────────────────────────────────────────────────────┤
│                    Application Layer                         │
│              (Use Cases - Business Logic)                    │
├─────────────────────────────────────────────────────────────┤
│                      Domain Layer                            │
│            (Entities - Aggregates - Repositories)            │
├─────────────────────────────────────────────────────────────┤
│                   Infrastructure Layer                       │
│              (EF Core - Database Access)                     │
├─────────────────────────────────────────────────────────────┤
│                      0_Framework                             │
│              (Shared - Cross-Cutting Concerns)               │
└─────────────────────────────────────────────────────────────┘
```

---

## ماژول‌های اصلی (Bounded Contexts)

### 1. ShopManagement (مدیریت فروشگاه)
مسئول محصولات، دسته‌بندی‌ها، اسلایدها و سفارشات

| پروژه | توضیح |
|-------|-------|
| `ShopManagement.Domain` | Entity ها: Product, ProductCategory, ProductPicture, Slide, Order |
| `ShopManagement.Application.Contracts` | Interface ها و DTO ها |
| `ShopManagement.Application` | پیاده‌سازی Use Case ها |
| `ShopManagement.Infrastructure.EFCore` | DbContext و Repository ها |
| `ShopManagement.Configuration` | Bootstrapper و Permissions |
| `ShopManagement.Presentation.Api` | API Controller ها |

### 2. AccountManagement (مدیریت کاربران)
مسئول احراز هویت، نقش‌ها و دسترسی‌ها

| پروژه | توضیح |
|-------|-------|
| `AccountManagement.Domain` | Entity ها: Account, Role, Permission |
| `AccountManagement.Application.Contracts` | Interface ها و DTO ها |
| `AccountManagement.Application` | پیاده‌سازی Use Case ها |
| `AccountMangement.Infrastructure.EFCore` | DbContext و Repository ها |
| `AccountManagement.Configuration` | Bootstrapper |

### 3. InventoryManagement (مدیریت انبار)
مسئول موجودی و قیمت‌گذاری

| پروژه | توضیح |
|-------|-------|
| `InventoryManagement.Domain` | Entity: Inventory |
| `InventoryManagement.Application.Contract` | Interface ها و DTO ها |
| `InventoryManagement.Application` | پیاده‌سازی Use Case ها |
| `InventoryMangement.Infrastructure.EFCore` | DbContext و Repository ها |
| `InventoryManagement.Infrastructure.Configuration` | Bootstrapper و Permissions |

### 4. DiscountManagement (مدیریت تخفیف‌ها)
مسئول تخفیف مشتریان و همکاران

| پروژه | توضیح |
|-------|-------|
| `DiscountManagement.Domain` | Entity ها: CustomerDiscount, ColleagueDiscount |
| `DiscountManagement.Application.Contract` | Interface ها و DTO ها |
| `DiscountManagement.Application` | پیاده‌سازی Use Case ها |
| `DiscountManagement.Infrastructure.EFCore` | DbContext و Repository ها |
| `DiscountManagement.Configuration` | Bootstrapper |

### 5. BlogManagement (مدیریت وبلاگ)
مسئول مقالات و دسته‌بندی مقالات

| پروژه | توضیح |
|-------|-------|
| `BlogManagement.Domain` | Entity ها: Article, ArticleCategory |
| `BlogManagement.Application.Contracts` | Interface ها و DTO ها |
| `BlogManagement.Application` | پیاده‌سازی Use Case ها |
| `BlogManagement.Infrastructure.EFCore` | DbContext و Repository ها |
| `BlogManagement.Infrastructure.Configuration` | Bootstrapper |

### 6. CommentManagement (مدیریت نظرات)
مسئول نظرات محصولات و مقالات

| پروژه | توضیح |
|-------|-------|
| `CommentManagement.Domain` | Entity: Comment |
| `CommentManagement.Application.Contracts` | Interface ها و DTO ها |
| `CommentManagement.Application` | پیاده‌سازی Use Case ها |
| `CommnetManagement.Infrastructure.EFCore` | DbContext و Repository ها |
| `CommentManagement.Infrastructure.Configuration` | Bootstrapper |

---

## پروژه‌های مشترک

### 0_Framework
شامل کدهای مشترک بین همه ماژول‌ها:

```
0_Framework/
├── Application/
│   ├── AuthHelper.cs          # مدیریت احراز هویت و Cookie
│   ├── IPasswordHasher.cs     # رمزنگاری پسورد
│   ├── OperationResult.cs     # نتیجه عملیات
│   └── ApplicationMessages.cs # پیام‌های سیستم
├── Domain/
│   ├── EntityBase.cs          # کلاس پایه Entity ها
│   └── IRepository.cs         # Interface پایه Repository
└── Infrastructure/
    ├── RepositoryBase.cs      # پیاده‌سازی پایه Repository
    ├── Roles.cs               # تعریف نقش‌ها (1=Admin, 2=User, 3=ContentUploader)
    └── NeedsPermissionAttribute.cs # Attribute برای چک کردن Permission
```

### 01_LampshadeQuery
شامل Query های پیچیده برای نمایش در سایت:

```
01_LampshadeQuery/
├── Contracts/
│   ├── Product/IProductQuery.cs
│   ├── Slide/ISlideQuery.cs
│   └── ...
└── Query/
    ├── ProductQuery.cs
    ├── SlideQuery.cs
    └── ...
```

---

## DbContext ها

| Context | جداول | فایل |
|---------|--------|------|
| `ShopContext` | Products, ProductCategories, ProductPictures, Slides, Orders | ShopManagement.Infrastructure.EFCore |
| `AccountContext` | Accounts, Roles, Permissions | AccountMangement.Infrastructure.EFCore |
| `InventoryContext` | Inventory | InventoryMangement.Infrastructure.EFCore |
| `DiscountContext` | CustomerDiscounts, ColleagueDiscounts | DiscountManagement.Infrastructure.EFCore |
| `BlogContext` | Articles, ArticleCategories | BlogManagement.Infrastructure.EFCore |
| `CommentContext` | Comments | CommnetManagement.Infrastructure.EFCore |

---

## سیستم احراز هویت و دسترسی

### نقش‌ها (Roles)
```csharp
public static class Roles
{
    public const string Administrator = "1";    // مدیر سیستم
    public const string SystemUser = "2";       // کاربر عادی
    public const string ContentUploader = "3";  // محتوا گذار
}
```

### Permission ها
هر عملیات یک کد عددی دارد:
```csharp
// ShopPermissions
public const int ListProducts = 10;
public const int CreateProduct = 12;
public const int ListProductCategories = 20;
public const int CreateProductCategory = 22;
// ...
```

### نحوه کار
1. کاربر لاگین می‌کند → `AuthHelper.Signin()` اطلاعات را در Cookie ذخیره می‌کند
2. هر صفحه با `[NeedsPermission(code)]` مشخص می‌کند چه Permission نیاز دارد
3. `SecurityPageFilter` قبل از اجرای هر صفحه، Permission کاربر را چک می‌کند

---

## ساختار ServiceHost (Presentation)

```
ServiceHost/
├── Areas/
│   └── Administration/        # پنل مدیریت
│       └── Pages/
│           ├── Shop/          # مدیریت فروشگاه
│           ├── Inventory/     # مدیریت انبار
│           ├── Discounts/     # مدیریت تخفیف‌ها
│           ├── Blog/          # مدیریت وبلاگ
│           ├── Comments/      # مدیریت نظرات
│           └── Accounts/      # مدیریت کاربران
├── Pages/                     # صفحات عمومی سایت
├── ViewComponents/            # کامپوننت‌های View
├── wwwroot/                   # فایل‌های استاتیک
├── Startup.cs                 # تنظیمات DI و Middleware
├── DatabaseSeeder.cs          # Seed داده‌های اولیه
└── appsettings.json           # تنظیمات
```

---

## نحوه اضافه کردن فیچر جدید

### مثال: اضافه کردن Entity جدید به ShopManagement

1. **Domain**: ساخت Entity در `ShopManagement.Domain/NewEntityAgg/`
2. **Contracts**: ساخت Interface و DTO در `ShopManagement.Application.Contracts/NewEntity/`
3. **Application**: پیاده‌سازی در `ShopManagement.Application/NewEntityApplication.cs`
4. **Infrastructure**: 
   - اضافه کردن DbSet به `ShopContext`
   - ساخت Mapping در `Mappings/`
   - ساخت Repository در `Repository/`
5. **Configuration**: ثبت در `ShopManagementBootstrapper.Configure()`
6. **Migration**: `dotnet ef migrations add NewEntity --project ShopManagement.Infrastructure.EFCore --startup-project ServiceHost --context ShopContext`
7. **Presentation**: ساخت صفحات در `ServiceHost/Areas/Administration/Pages/Shop/NewEntity/`

---

## نکات مهم

- **دیتابیس**: SQLite (`lampshade.db` در پوشه ServiceHost)
- **Connection String**: در `appsettings.json` → `Data Source=lampshade.db`
- **احراز هویت**: Cookie-based با ASP.NET Core Identity
- **پسورد پیش‌فرض ادمین**: `admin` / `admin123`
