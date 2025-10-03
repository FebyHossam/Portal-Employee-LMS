# 🏢 Portal Employee Leave Management System (LMS)

A web-based application for **Employee Leave Management**, built with **ASP.NET Core MVC** and **Entity Framework Core**.  
It aims to simplify leave requests and approvals within organizations, with different roles and permissions for Employees, Managers, and HR.

---

## ✨ Key Features

- 📝 Employees can submit leave requests  
- ✅ Managers/HR can approve or reject requests  
- 🔑 Role-based access (HR / Manager / Employee)  
- 📊 Track all leave requests with reports  
- 🗄️ SQL Server database with EF Core  
- 🎨 User-friendly UI built with Razor Pages / MVC  

---

## 📂 Project Structure
/
│Portal-Employee-LMS/
├── Controllers/ ← Application controllers
├── Data/ ← Database context and configuration
├── Migrations/ ← EF Core migrations
├── Models/ ← Entity models
├── Service/ ← Business logic services
├── ViewModels/ ← DTOs & View Models
├── Views/ ← Razor views (UI)
├── wwwroot/ ← Static files (CSS, JS, Images)
├── appsettings.json ← App configuration & DB connection
├── Program.cs ← Application entry point
└── *.sln / *.csproj ← Solution and project files

## 🛠 Requirements

- **.NET 6.0** or later  
- **SQL Server**  
- Visual Studio 2022 or VS Code  
- EF Core CLI tools  

---
## 🚀 Getting Started

1. **Clone the repository**  
   ```bash
   git clone https://github.com/FebyHossam/Portal-Employee-LMS.git
Open the project in Visual Studio or VS Code

Update the database connection string in appsettings.json

Apply migrations to create the database:

bash
Copy code
dotnet ef database update
Run the application

Using Visual Studio: Run (F5)

Using CLI:

bash
Copy code
dotnet run
Open your browser at:

arduino
Copy code
https://localhost:5001
