# BulBite - ASP.NET Core MVC N-Layer Sample

## Architecture
Controller -> Service Interface -> Service Implementation -> Repository Interface -> Repository Implementation -> ApplicationDbContext -> SQL Server

## Requirements
- Visual Studio 2022
- .NET 8 SDK
- SQL Server / SQL Express / LocalDB
- SSMS

## 1. Configure SQL Server
Edit `appsettings.json`:
`Server=localhost;Database=BulBiteDb;Trusted_Connection=True;TrustServerCertificate=True;`

If your SSMS server name is different, replace `localhost`.

## 2. Open
Open `AuthMvcApp.csproj` in Visual Studio.

## 3. Restore/build
Build -> Rebuild Solution.

Or:
`dotnet restore`
`dotnet build`

## 4. Database
The application currently calls `EnsureCreated()` at startup, so the `BulBiteDb` database and `Users`/`Products` tables can be created automatically when the SQL login has permission.

You can also create the database manually in SSMS:
`CREATE DATABASE BulBiteDb;`

For production, replace `EnsureCreated()` with EF Core migrations.

## 5. Test login
1. Run the app.
2. Open Register.
3. Create an account.
4. Check `BulBiteDb -> dbo -> Users` in SSMS.
5. Login using the same email/password.
6. Successful login creates an authentication cookie and redirects to `/Dashboard/Index`.
7. Dashboard and Products are protected with `[Authorize]`.

## 6. Test data flow
Use Dashboard -> Add Product.
The flow is:
ProductController -> IProductService -> ProductService -> IProductRepository -> ProductRepository -> ApplicationDbContext -> SQL Server.

Products can then be viewed/edited/deleted from the UI.

## Important
Passwords are never stored as plain text. `PasswordHasher<User>` stores a secure password hash.
