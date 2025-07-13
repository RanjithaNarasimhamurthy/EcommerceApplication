## Ecommerce Application - .NET MAUI
A cross-platform mobile shopping application built with .NET MAUI (Android & iOS) and an ASP.NET Core Web API back-end.  
Users can browse products, manage a cart, place orders, and authenticate via OTP (SMS/email).
Built a cross-platform shopping application targeting Android and iOS from a single .NET MAUI C# codebase, delivering native performance and consistent UX.
Implemented password-less OTP authentication via Twilio SMS and MailKit-sent email, simplifying onboarding while maintaining strong security.
Developed an end-to-end REST API in ASP.NET Core with controllers for products, categories, cart, orders, user profiles & addresses, vendor uploads, and customer feedback.
Architected the client using MVVM with XAML views bound to ViewModels and dependency-injected HTTP services, ensuring clean separation of concerns and maintainable UI code.


---

## Features

- **User Authentication**  
  - OTP via Twilio SMS  
  - Email verification via SMTP (MailKit)  
- **Product Catalog**  
  - Listing, searching, filtering by category  
- **Shopping Cart & Orders**  
  - Add/remove items, view cart, place orders  
- **Offline Support**  
  - Cache product list & cart with SQLite for offline browsing  
- **Admin/Vendor Endpoints**  
  - Manage products, inventory, and customer feedback  
- **CI/CD Integration**  
  - Automated build & test pipelines with GitHub Actions  

---

## Tech Stack

- **Mobile Client**  
  - [.NET MAUI](https://docs.microsoft.com/dotnet/maui/) (C#, XAML, MVVM)  
  - [SQLite](https://www.sqlite.org/) for local caching  
- **Server API**  
  - [ASP.NET Core Web API](https://docs.microsoft.com/aspnet/core/web-api/) (.NET 7)  
  - [Twilio](https://www.twilio.com/) SDK for SMS OTP  
  - [MailKit](https://github.com/jstedfast/MailKit) for SMTP email OTP  
- **CI/CD**  
  - [GitHub Actions](https://github.com/features/actions)  

---

