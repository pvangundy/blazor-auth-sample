# Blazor Auth Sample

A template solution that pairs a Blazor WebAssembly mobile-first frontend with an ASP.NET Core Web API backend. Both tiers are protected with Azure Entra ID (Azure Active Directory) and the backend uses the Dapper micro ORM for data access. The frontend styling leans on Tailwind CSS to achieve an iOS-inspired aesthetic.

## Solution structure

```
BlazorAuthSample.sln
└── src
    ├── Client           # Blazor WebAssembly project (Tailwind CLI, Azure Entra auth)
    │   ├── Features     # Feature-sliced UI modules
    │   ├── Shared       # Layout and reusable UI elements
    │   └── Styles       # Tailwind input file
    ├── Server           # ASP.NET Core Web API with Azure Entra + Dapper
    │   ├── Features     # Feature-sliced API endpoints and services
    │   └── Infrastructure
    └── Shared           # Cross-project models (e.g., WeatherForecast)
```

## Prerequisites

* .NET 8 SDK
* Node.js 18+ (for Tailwind CLI)
* An Azure Entra ID tenant with permissions to create app registrations
* (Optional) SQL Server or Azure SQL Database if you plan to connect to a real database

## Azure Entra ID configuration

The template expects two Azure app registrations – one for the WebAssembly client and one for the Web API.

### 1. Create the API app registration

1. In the Azure portal, navigate to **Azure Active Directory ➜ App registrations ➜ New registration**.
2. Name it (e.g., `BlazorAuthSample-API`) and set the supported account types as needed.
3. After creation, capture the **Application (client) ID** and **Directory (tenant) ID**.
4. Under **Expose an API**, add an Application ID URI (e.g., `api://{api-client-id}`) and create a scope (e.g., `user_impersonation`).
5. Update `src/Server/appsettings.json` with:
   * `TenantId`
   * `ClientId`
   * `Audience` (`api://{api-client-id}`)
6. Ensure the API has the `user_impersonation` scope exposed.

### 2. Create the client app registration

1. Register another application (e.g., `BlazorAuthSample-Client`).
2. Set the platform to **Single-page application (SPA)** and add redirect URIs:
   * `https://localhost:5002/authentication/login-callback`
   * `https://localhost:5002/authentication/logout-callback`
3. Under **Authentication**, enable “Access tokens” and “ID tokens”.
4. Under **API permissions**, add a delegated permission for the API scope you created earlier (e.g., `user_impersonation`) and grant admin consent.
5. Update `src/Client/wwwroot/appsettings.json` with:
   * `Authority` (`https://login.microsoftonline.com/{tenantId}`)
   * `ClientId` (client app registration id)
   * `DefaultScope` (`api://{api-client-id}/user_impersonation`)
   * `BaseUrl` for the API under the `Api` section if different from the default.

### 3. Configure cross-origin access

The backend’s `ClientUrl` value in `src/Server/appsettings.json` must match the SPA origin. Update it if you host the WebAssembly app elsewhere.

## Running the solution

1. Restore NuGet packages and install Tailwind dependencies:

   ```bash
   dotnet restore
   cd src/Client
   npm install
   npm run tailwind:build
   cd ../../
   ```

2. Build the solution:

   ```bash
   dotnet build BlazorAuthSample.sln
   ```

3. Run the Web API:

   ```bash
   dotnet run --project src/Server/Server.csproj
   ```

4. In another terminal, run the Blazor WebAssembly dev server:

   ```bash
   dotnet run --project src/Client/Client.csproj
   ```

   Navigate to `https://localhost:5002` (update URLs if you change launch profiles).

### Tailwind build automation

The `Client.csproj` contains MSBuild targets that automatically run `npm install` and `npm run tailwind:build` before each `dotnet build`. You can skip this by setting the `RunTailwind` property to `false`:

```bash
dotnet build src/Client/Client.csproj -p:RunTailwind=false
```

### Database connectivity

The repository uses a SQL Server connection via Dapper. Update the `DefaultConnection` string in `src/Server/appsettings.json` to point to your database. The sample query falls back to generated data if the connection cannot be established, making it safe to run without a database during development.

## Feature-sliced organization

* **Client** – Each feature (e.g., `Home`, `Weather`) lives under `Client/Features/<FeatureName>` and exposes its own components and services.
* **Server** – API endpoints live in `Server/Features/<FeatureName>`. Each feature can have its controller, repository/service, and supporting logic alongside feature-specific SQL or mappings.
* **Shared** – Common contracts/models shared between projects.

This structure helps you scale by keeping related UI, API, and shared contracts grouped together.

## Extending the template

* Add new features by creating matching folders under `Client/Features` and `Server/Features`, along with shared models where necessary.
* Register additional repositories/services in `Program.cs` similar to the weather example.
* Update Tailwind styles via `src/Client/Styles/app.css` and rebuild using `npm run tailwind:build`.

## Security considerations

* Treat all placeholders (`{tenantId}`, `{apiAppId}`, etc.) as required configuration values and replace them before deploying.
* For production, configure HTTPS certificates, cookie policies, and strict CORS rules tailored to your deployment environment.
* Store secrets (like connection strings) securely using [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) or Azure Key Vault.

## Useful commands

```bash
# Restore all projects
dotnet restore

# Build and run Tailwind in watch mode (frontend)
cd src/Client
npm run tailwind:watch

# Run tests (add your test projects first)
dotnet test
```

Happy building!
