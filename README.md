# Blazor Auth Sample

This repository provides a starter template for building a mobile-first Blazor WebAssembly application secured with Azure Entra ID (Azure AD) and backed by an ASP.NET Core Web API that uses Dapper for data access. The UI leans on Tailwind CSS to achieve a polished iOS aesthetic.

## Solution structure

```
src/
 ├─ Client/   # Blazor WebAssembly frontend (Tailwind CLI, MSAL auth)
 ├─ Server/   # ASP.NET Core Web API (Azure AD auth, Dapper repositories)
 └─ Shared/   # Shared contracts and DTOs
```

Both the client and server follow a feature-slice layout where features own their components, services, and endpoints.

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/) for building and running the solution.
* [Node.js 18+](https://nodejs.org/) for executing the Tailwind CLI build.
* An Azure Entra ID tenant with permissions to create app registrations.
* (Optional) SQL Server or Azure SQL Database instance to host the `WeatherForecast` table consumed by the sample repository.

## First run

```bash
# Restore .NET dependencies
dotnet restore src/BlazorAuthSample.sln

# Build Tailwind output (runs automatically on build, but can be run manually)
cd src/Client
npm install
npm run build:css
cd ../..

# Build the solution
dotnet build src/BlazorAuthSample.sln
```

During development run the projects from two terminals:

```bash
# Terminal 1
cd src/Server
dotnet run

# Terminal 2
cd src/Client
dotnet run
```

The server exposes Swagger UI at `https://localhost:5001/swagger` for quick inspection.

## Tailwind build pipeline

Tailwind input lives in `src/Client/Styles/app.css`. The generated bundle is placed in `wwwroot/css/app.css`. A custom MSBuild target in `Client.csproj` ensures `npm install` and Tailwind compilation execute before every .NET build. For faster iteration you can also run `npm run watch:css` inside `src/Client`.

## Azure Entra ID configuration

Follow these steps to wire up authentication:

1. **Create the server app registration**
   - In the Azure portal, register a new application (e.g., `BlazorAuthSample-Server`).
   - Set the application to be *Accounts in this organizational directory only*.
   - Expose an API with the Application ID URI `api://<backend-client-id>` (replace with the actual application ID).
   - Add a delegated permission scope (e.g., `access_as_user`) and grant admin consent.
   - Record the *Application (client) ID*, *Directory (tenant) ID*, and configured scope name.

2. **Create the client app registration**
   - Register another application (e.g., `BlazorAuthSample-Client`).
   - Set the redirect URI type to *Single-page application (SPA)* and use `https://localhost:5002/authentication/login-callback`.
   - Under *Authentication*, enable **Access tokens** and **ID tokens**.
   - Configure *API permissions* to include the delegated permission created for the server API and grant consent.
   - Record the *Application (client) ID*.

3. **Configure the projects**
   - Update `src/Server/appsettings.json`:
     * `TenantId`: directory (tenant) ID.
     * `ClientId`: server app registration client ID.
     * `Audience`: `api://<backend-client-id>` matching the exposed API URI.
     * `Domain`: primary domain of the tenant (e.g., `contoso.onmicrosoft.com`).
   - Update `src/Client/wwwroot/appsettings.json`:
     * `Authority`: `https://login.microsoftonline.com/<tenant-id>`.
     * `ClientId`: client app registration client ID.
     * `Scopes`: `["api://<backend-client-id>/<scope-name>"]` (for example `api://00000000-0000-0000-0000-000000000000/access_as_user`).
     * `BaseUrl`: URL where the Web API is hosted (defaults to `https://localhost:5001`).

4. **Expose CORS for local development**
   - Ensure `ClientApps:Blazor` in `src/Server/appsettings.json` matches the HTTPS origin used by the WASM app (default `https://localhost:5002`).

5. **Database preparation (optional)**
   - Create a `WeatherForecast` table with columns `Date` (`datetime`), `TemperatureC` (`int`), and `Summary` (`nvarchar(128)`).
   - Seed a few rows so that the template endpoint can return data.

## Feature slice layout

Each feature owns its API endpoints, data access, and UI surface:

* `Server/Features/Weather` contains the Dapper repository and minimal API endpoint.
* `Client/Features/Weather` groups the pages, services, and components that call the backend.
* `Client/Features/Dashboard` contains the home dashboard UI.

This structure keeps related assets together and scales cleanly as more features are added.

## Extending the template

* Add new secured API endpoints by creating a folder under `Server/Features/<FeatureName>` with a repository/service and endpoint mapping extension.
* Create matching UI pages under `Client/Features/<FeatureName>` and register any new API clients or services in `Program.cs`.
* Leverage Tailwind utility classes (or extend `tailwind.config.cjs`) to keep the mobile-first, iOS-inspired look and feel consistent.

## Troubleshooting

* If authentication fails locally, confirm the redirect URI and SPA configuration match the local client URL and that the exposed API scope has consent.
* When running behind HTTPS with a self-signed certificate, trust the certificate generated by `dotnet dev-certs https --trust` on your machine.
* Delete `wwwroot/css/app.css` and rebuild Tailwind if you encounter stale styling.
