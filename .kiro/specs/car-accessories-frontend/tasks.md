# Implementation Plan

- [x] 1. راه‌اندازی تم جدید و Layout





  - [x] 1.1 ایجاد پوشه تم جدید در wwwroot/Theme/CarOption


    - ایجاد ساختار پوشه‌ها (css, js, images)
    - کپی تنظیمات Tailwind از front_car_store
    - _Requirements: 8.3_
  - [x] 1.2 به‌روزرسانی _Layout.cshtml با Tailwind CSS


    - تغییر از Bootstrap به Tailwind CSS CDN
    - اضافه کردن تنظیمات رنگ و فونت
    - اضافه کردن فونت Vazirmatn از Google Fonts
    - تنظیم dir="rtl" و lang="fa"
    - _Requirements: 8.1, 8.3_

- [x] 2. پیاده‌سازی هدر جدید (Menu Component)





  - [x] 2.1 به‌روزرسانی Menu/Default.cshtml با طراحی جدید


    - هدر sticky با backdrop-blur
    - لوگو با آیکون directions_car و نام "فروشگاه آپشن"
    - منوی ناوبری با لینک به دسته‌بندی‌ها
    - باکس جستجو در مرکز
    - آیکون سبد خرید با شمارنده
    - دکمه ورود/ثبت‌نام
    - _Requirements: 2.1, 2.2, 2.3_

  - [x] 2.2 پیاده‌سازی منوی موبایل

    - منوی همبرگری برای موبایل
    - باکس جستجوی موبایل
    - _Requirements: 2.4, 6.4_
  - [ ]* 2.3 نوشتن property test برای Layout Consistency
    - **Property 1: Page Layout Consistency**
    - **Validates: Requirements 2.1, 7.1**

- [x] 3. پیاده‌سازی فوتر جدید (Footer Component)





  - [x] 3.1 به‌روزرسانی Footer/Default.cshtml با طراحی جدید


    - فوتر با پس‌زمینه surface-dark
    - اطلاعات تماس و کپی‌رایت
    - آیکون‌های شبکه‌های اجتماعی
    - _Requirements: 7.1, 7.2_

- [x] 4. Checkpoint - اطمینان از صحت Layout





  - Ensure all tests pass, ask the user if questions arise.

- [x] 5. پیاده‌سازی صفحه اصلی (Index)





  - [x] 5.1 به‌روزرسانی Slide/Default.cshtml با طراحی جدید


    - اسلایدر با طراحی تاریک
    - نمایش محصولات ویژه
    - _Requirements: 1.3_
  - [x] 5.2 به‌روزرسانی ProductCategory/Default.cshtml با طراحی جدید


    - کارت‌های دسته‌بندی با hover effect
    - نمایش سه دسته‌بندی (مانیتور، دوربین، فریم)
    - _Requirements: 1.1, 1.2_
  - [x] 5.3 به‌روزرسانی Index.cshtml با طراحی جدید


    - چیدمان صفحه اصلی
    - بخش محصولات پرفروش
    - بنرهای تبلیغاتی
    - _Requirements: 1.4_

- [-] 6. پیاده‌سازی کارت محصول (Partial View)



  - [x] 6.1 ایجاد _ProductCard.cshtml در Pages/Shared



    - تصویر با aspect-ratio 4:3
    - برچسب تخفیف یا جدید
    - دکمه‌های علاقه‌مندی و مقایسه (hover)
    - نام محصول با hover effect
    - قیمت با نمایش تخفیف
    - امتیاز ستاره‌ای
    - دکمه افزودن به سبد
    - _Requirements: 4.1, 4.2, 4.3, 4.4_
  - [ ]* 6.2 نوشتن property test برای Product Card
    - **Property 5: Product Card Information Completeness**
    - **Validates: Requirements 4.1**
  - [ ]* 6.3 نوشتن property test برای Discount Display
    - **Property 6: Discount Display Correctness**
    - **Validates: Requirements 4.2**

- [x] 7. Checkpoint - اطمینان از صحت صفحه اصلی





  - Ensure all tests pass, ask the user if questions arise.

- [x] 8. پیاده‌سازی صفحه دسته‌بندی (ProductCategory)






  - [x] 8.1 به‌روزرسانی ProductCategory.cshtml با طراحی جدید

    - Breadcrumb navigation
    - سایدبار فیلترها (دسته‌بندی، قیمت، برند خودرو)
    - گرید محصولات با استفاده از _ProductCard
    - مرتب‌سازی (پرفروش‌ترین، جدیدترین، ارزان‌ترین، گران‌ترین)
    - صفحه‌بندی
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_
  - [x] 8.2 پیاده‌سازی طراحی ریسپانسیو


    - تک‌ستونه در موبایل
    - دو‌ستونه در تبلت
    - سه‌ستونه در دسکتاپ
    - _Requirements: 6.1, 6.2, 6.3_
  - [ ]* 8.3 نوشتن property test برای Sort Order
    - **Property 3: Sort Order Correctness**
    - **Validates: Requirements 3.3**
  - [ ]* 8.4 نوشتن property test برای Pagination
    - **Property 4: Pagination Threshold**
    - **Validates: Requirements 3.4**

- [x] 9. پیاده‌سازی صفحه جزئیات محصول (Product)





  - [x] 9.1 به‌روزرسانی Product.cshtml با طراحی جدید


    - گالری تصاویر با thumbnail
    - اطلاعات محصول (نام، کد، قیمت، موجودی)
    - امتیاز و تعداد دیدگاه
    - دکمه افزودن به سبد
    - لیست خودروهای سازگار
    - _Requirements: 5.1, 5.2, 5.4_

  - [ ] 9.2 پیاده‌سازی تب‌های محصول
    - تب توضیحات محصول
    - تب مشخصات فنی
    - تب دیدگاه کاربران

    - _Requirements: 5.3_
  - [ ] 9.3 پیاده‌سازی بخش محصولات مشابه
    - نمایش محصولات مشابه در پایین صفحه
    - _Requirements: 5.5_
  - [ ]* 9.4 نوشتن property test برای Product Detail
    - **Property 7: Product Detail Information Completeness**
    - **Validates: Requirements 5.2**

- [x] 10. Checkpoint - اطمینان از صحت همه صفحات











  - Ensure all tests pass, ask the user if questions arise.

- [x] 11. به‌روزرسانی سایر کامپوننت‌ها







  - [x] 11.1 به‌روزرسانی LatestArrivals/Default.cshtml


    - استفاده از _ProductCard
    - طراحی گرید جدید
    - _Requirements: 1.4_
  - [x] 11.2 به‌روزرسانی ProductCategoryWithProduct/Default.cshtml


    - استفاده از _ProductCard
    - طراحی گرید جدید
    - _Requirements: 1.1_

- [x] 12. Final Checkpoint - اطمینان از صحت کل سیستم





  - Ensure all tests pass, ask the user if questions arise.
