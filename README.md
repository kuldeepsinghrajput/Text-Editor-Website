# Text Storage Application

An ASP.NET Core MVC web application that allows registered users to create, store, view, and manage text records securely. Each user can access only their own saved text.

## Features

- User registration, login, and logout.
- Secure authentication using ASP.NET Core Identity.
- Create and save text records with a title and content.
- View saved text records.
- Inline editing of titles and content.
- Automatic saving when leaving an edited field.
- User-specific data access.
- Server-side validation for required fields.
- SQL Server database integration using Entity Framework Core.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| .NET 10 | Application framework |
| ASP.NET Core MVC | Web application architecture |
| Razor Views | User interface |
| Entity Framework Core 10 | Database access and ORM |
| ASP.NET Core Identity | Authentication and user management |
| SQL Server LocalDB | Local database |
| HTML, CSS, JavaScript | Frontend and inline editing |
| Visual Studio | Development environment |
| Git and GitHub | Version control and source hosting |

## Prerequisites

Install the following software before running the application:

1. **.NET 10 SDK** — [Download .NET](https://dotnet.microsoft.com/download/dotnet/10.0)
2. **Visual Studio 2022 or a compatible newer Visual Studio release** with the ASP.NET and web development workload.
3. **SQL Server LocalDB** — typically available through Visual Studio's SQL Server/Data storage components. Verify that LocalDB is installed.
4. **Git** — [Download Git](https://git-scm.com/downloads)

> SQL Server Management Studio (SSMS) is optional. It is useful for inspecting the database, but it is not required to run the application. SSMS alone does not install the SQL Server database engine or LocalDB.

## Installation and Setup

### 1. Clone the Repository

Open a terminal and run:

```bash
git clone <YOUR_GITHUB_REPOSITORY_URL>
cd <YOUR_REPOSITORY_FOLDER>
```

Replace the placeholders with your repository URL and folder name.

### 2. Verify the .NET SDK

```bash
dotnet --list-sdks
```

Ensure a .NET 10 SDK is installed.

### 3. Restore and Install NuGet Packages

The project dependencies are defined in the `.csproj` file. When you clone the repository, NuGet packages are restored automatically when you build the project in Visual Studio or run `dotnet restore` from the terminal.

#### Option 1: Restore packages automatically (Recommended)

Navigate to the folder containing the project file and run:

```bash
dotnet restore
```

Then build the project:

```bash
dotnet build
```

#### Option 2: Install packages manually

If any required package is missing from the project, install it from the terminal in the project directory:

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 10.0.0
dotnet add package Microsoft.AspNetCore.Identity.UI --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.0
```

These packages provide:

| NuGet package | Purpose |
|---|---|
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Stores Identity users and authentication data using Entity Framework Core. |
| `Microsoft.AspNetCore.Identity.UI` | Provides the default Identity UI, including login and registration pages when configured. |
| `Microsoft.EntityFrameworkCore.SqlServer` | Connects Entity Framework Core to SQL Server and LocalDB. |
| `Microsoft.EntityFrameworkCore.Tools` | Provides Entity Framework Core tooling for migrations and database management in Visual Studio. |

**Note:** The package versions above are examples for a .NET 10 project. Keep package versions compatible with your target framework and the other Entity Framework Core packages. If these package references already exist in the `.csproj` file, do not add them again.

#### Using Visual Studio

You can also install packages through the IDE:

1. Right-click the project in **Solution Explorer**.
2. Select **Manage NuGet Packages**.
3. Open the **Browse** tab.
4. Search for the required package.
5. Select a compatible version and click **Install**.

After installing or restoring the packages, continue with the database connection configuration and Entity Framework Core migrations.

### 4. Configure the Database Connection

Open `appsettings.json` and verify the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TextStorageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

This configuration uses SQL Server LocalDB and creates or connects to a database named `TextStorageDb`.

If you use a different SQL Server instance, update the connection string accordingly.

### 5. Verify LocalDB

Open a terminal and execute:

```bash
sqllocaldb info
```

If `MSSQLLocalDB` is listed, start it if necessary:

```bash
sqllocaldb start MSSQLLocalDB
```

If the command is unavailable or the instance is missing, install SQL Server Express LocalDB.

### 6. Apply Entity Framework Core Migrations

If the repository contains migrations, open the terminal in the project directory and run:

```bash
dotnet ef database update
```

If the Entity Framework CLI is not installed, install it with:

```bash
dotnet tool install --global dotnet-ef
```

If it is already installed, update it if required:

```bash
dotnet tool update --global dotnet-ef
```

If the repository does not contain migrations, create an initial migration before updating the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Do not create another initial migration if the existing migrations already define the database schema.

### 7. Run the Application

Start the application from the directory containing the `.csproj` file:

```bash
dotnet run
```

Alternatively, open the project in Visual Studio and press **F5** or **Ctrl+F5**.

Open the HTTP or HTTPS URL displayed in the terminal.

### 8. Register and Test

1. Open the application in your browser.
2. Register a new user account.
3. Log in with the registered account.
4. Create a text record with a title and content.
5. Open the saved record and edit its title or content.
6. Verify that changes are saved when you leave the editing field.
7. Log out and register or log in as another user to verify that records are isolated between accounts.

## Project Structure

```text
test_project/
├── Controllers/
│   ├── HomeController.cs
│   └── SavedTextsController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── ErrorViewModel.cs
│   └── SavedText.cs
├── Views/
│   ├── Home/
│   ├── SavedTexts/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Details.cshtml
│   └── Shared/
├── Areas/
│   └── Identity/
├── Migrations/
├── wwwroot/
├── appsettings.json
├── Program.cs
└── README.md
```

*The structure above is representative. The exact files and folders may differ depending on the current repository.*

## Database Design

### SavedTexts

| Column | Description |
|---|---|
| `Id` | Primary key for each saved text record |
| `Title` | Title of the saved text |
| `Content` | Stored text content |
| `CreatedAt` | Record creation timestamp |
| `UserId` | Identity user ID associated with the record |

The application uses Entity Framework Core to access SQL Server. ASP.NET Core Identity manages user accounts and authentication.

## Security

- Authentication is managed through ASP.NET Core Identity.
- Protected controller actions require an authenticated user.
- Saved records are filtered by the current user's ID.
- Updates verify record ownership before saving changes.
- Anti-forgery validation is used for applicable POST requests.
- Do not commit production passwords, API keys, or other secrets to GitHub.

## Troubleshooting

**Database connection fails**
- Confirm that SQL Server LocalDB is installed and running.
- Verify the `DefaultConnection` value in `appsettings.json`.
- Ensure the database migrations have been applied.

**`dotnet ef` is not recognized**
- Install the Entity Framework CLI tool.
- Restart the terminal if the global tool path was updated.

**Missing database tables**
- Run `dotnet ef database update` from the project directory.
- Check that the migrations are included in the repository.

**Login or registration fails**
- Verify that Identity services are registered in `Program.cs`.
- Ensure `ApplicationDbContext` inherits from `IdentityDbContext`.
- Confirm that the database migration has created the Identity tables.

## Future Improvements

- Search and filter saved text records.
- Add pagination for large collections.
- Add text export and download functionality.
- Improve responsive UI and accessibility.
- Add automated unit and integration tests.
- Deploy the application to Azure App Service with a production database.

## License

No license has been specified yet. Add a `LICENSE` file if you intend to permit others to reuse or modify this project.
