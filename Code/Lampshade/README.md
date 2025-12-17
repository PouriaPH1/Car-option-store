# Lampshade - فروشگاه آنلاین

یک فروشگاه آنلاین با ASP.NET Core 8.0، Entity Framework Core و SQLite

## پیش‌نیازها

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [EF Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

---

## راه‌اندازی سریع

### 1. Clone و Restore
```bash
cd Code/Lampshade
dotnet restore
```

### 2. ساخت دیتابیس (اگر `lampshade.db` وجود ندارد)
```bash
# اجرای Migration ها برای همه Context ها
dotnet ef database update --project ShopManagement.Infrastructure.EFCore --startup-project ServiceHost --context ShopContext
dotnet ef database update --project AccountMangement.Infrastructure.EFCore --startup-project ServiceHost --context AccountContext
dotnet ef database update --project InventoryMangement.Infrastructure.EFCore --startup-project ServiceHost --context InventoryContext
dotnet ef database update --project BlogManagement.Infrastructure.EFCore --startup-project ServiceHost --context BlogContext
dotnet ef database update --project DiscountManagement.Infrastructure.EFCore --startup-project ServiceHost --context DiscountContext
dotnet ef database update --project CommnetManagement.Infrastructure.EFCore --startup-project ServiceHost --context CommentContext
```

### 3. اجرای پروژه
```bash
cd ServiceHost
dotnet run
```

### 4. دسترسی به سایت
- **سایت**: https://localhost:5001
- **پنل ادمین**: https://localhost:5001/Administration
- **صفحه لاگین**: https://localhost:5001/Account

---

## مشخصات ورود ادمین

| فیلد | مقدار |
|------|-------|
| Username | `admin` |
| Password | `admin123` |

> ⚠️ اگر اولین بار است که پروژه را اجرا می‌کنید، `DatabaseSeeder` به صورت خودکار کاربر ادمین را می‌سازد.

---

## ساختار پروژه

```
Lampshade/
├── 0_Framework/                    # کدهای مشترک
├── 01_LampshadeQuery/              # Query های سایت
├── AccountManagement.*/            # ماژول کاربران
├── BlogManagement.*/               # ماژول وبلاگ
├── CommentManagement.*/            # ماژول نظرات
├── DiscountManagement.*/           # ماژول تخفیف‌ها
├── InventoryManagement.*/          # ماژول انبار
├── ShopManagement.*/               # ماژول فروشگاه
├── ServiceHost/                    # پروژه اصلی (Presentation)
│   ├── lampshade.db               # دیتابیس SQLite
│   ├── appsettings.json           # تنظیمات
│   └── Startup.cs                 # تنظیمات DI
└── Lampshade.sln                  # Solution File
```

---

## دستورات مفید

### Build
```bash
dotnet build
```

### اجرا در حالت Development
```bash
cd ServiceHost
dotnet run --environment Development
```

### ساخت Migration جدید
```bash
# مثال برای ShopContext
dotnet ef migrations add MigrationName --project ShopManagement.Infrastructure.EFCore --startup-project ServiceHost --context ShopContext
```

### پاک کردن و ساخت مجدد دیتابیس
```bash
cd ServiceHost
Remove-Item lampshade.db -Force  # یا در Linux: rm lampshade.db
cd ..
# سپس دستورات database update را اجرا کنید
```

---

## تنظیمات

### Connection String
فایل `ServiceHost/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "LampshadeDb": "Data Source=lampshade.db"
  }
}
```

### تغییر به SQL Server (اختیاری)
1. پکیج‌ها را در فایل‌های `.csproj` تغییر دهید:
   ```xml
   <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
   ```
2. در Bootstrapper ها `UseSqlite` را به `UseSqlServer` تغییر دهید
3. Connection String را آپدیت کنید

---

## مستندات بیشتر

برای درک کامل معماری پروژه، فایل [ARCHITECTURE.md](./ARCHITECTURE.md) را مطالعه کنید.
