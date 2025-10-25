# Blazor Auth Sample

A template solution that pairs a Blazor WebAssembly frontend with an ASP.NET Core Web API backend. The solution is wired for Azure Entra ID authentication, uses Tailwind CSS (via the Tailwind CLI) for a mobile-first iOS inspired UI, and adopts feature-sliced organization for both client and server code. Data access is implemented with the Dapper micro-ORM.

## Solution structure

```
blazor-auth-sample.sln
├── src
│   ├── Client/              # Blazor WebAssembly app (mobile-first, Tailwind-powered)
│   ├── Server/              # ASP.NET Core Web API with Azure Entra protection
│   └── Shared/              # Cross-cutting contracts (DTOs, shared models)
```

Each application organizes functionality by feature slices (`Features/<FeatureName>`). Supporting infrastructure (for example, Dapper connection factories) lives alongside the features that depend on it.

## Getting started

1. **Install prerequisites**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download)
   - [Node.js 18+](https://nodejs.org/en/download) (for Tailwind CLI)

2. **Restore .NET dependencies**

   ```bash
   dotnet restore
   ```

3. **Install Tailwind CLI dependencies** (run inside `src/Client`):

   ```bash
   cd src/Client
   npm install
   ```

4. **Build Tailwind CSS**

   ```bash
   npm run build:css
   ```

   > The Blazor project automatically runs this script before every build (see the custom MSBuild target in `Client.csproj`). Running it manually during development enables hot-reload scenarios.

5. **Run the solution**

   ```bash
   dotnet build
   dotnet run --project src/Server/Server.csproj
   dotnet run --project src/Client/Client.csproj
   ```

   The client is configured to call the API over HTTPS on port `5001` by default. Update `src/Client/wwwroot/appsettings.json` if you host the API elsewhere.

## Configuring Azure Entra ID authentication

### 1. Create app registrations

Create two app registrations in the Azure portal (Azure Active Directory ➜ App registrations):

- **Server (Web API) app registration**
  - Supported account type: choose according to your requirements.
  - Expose an API ➜ Add a scope (for example, `access_as_user`). Record the Application (client) ID and Directory (tenant) ID.
  - Update the `appsettings.json` file in the Server project:

    ```json
    {
      "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "Domain": "<tenant-domain>",
        "TenantId": "<tenant-id>",
        "ClientId": "<server-client-id>",
        "Audience": "api://<server-client-id>"
      }
    }
    ```

- **Client (Blazor WebAssembly) app registration**
  - Redirect URIs ➜ Add a SPA redirect URI (e.g., `https://localhost:5002/authentication/login-callback`).
  - Allow public client flows.
  - Configure API permissions ➜ Add the scope you exposed on the API app registration and grant admin consent.
  - Update the client-side `appsettings.json`:

    ```json
    {
      "AzureAd": {
        "Authority": "https://login.microsoftonline.com/<tenant-id>",
        "ClientId": "<client-app-id>",
        "DefaultScope": "api://<server-client-id>/.default"
      }
    }
    ```

### 2. Optional: configure custom domains and logout URLs

- Update `Backend:BaseAddress` in `src/Client/wwwroot/appsettings.json` to match the server's public HTTPS endpoint.
- Update `App:ClientUrl` in `src/Server/appsettings.json` so CORS aligns with your frontend origin.
- Add additional redirect URIs (logout, silent renew) if you host the app on multiple origins.

### 3. Database configuration

- Configure the `ConnectionStrings:Default` entry in `src/Server/appsettings.json` for your SQL Server instance.
- Provision the `WeatherForecast` table schema expected by the template:

  ```sql
  CREATE TABLE WeatherForecast (
      [Date] date NOT NULL,
      [TemperatureC] int NOT NULL,
      [Summary] nvarchar(256) NOT NULL
  );
  ```

  Seed with sample data so the weather forecast feature has content after authentication.

### 4. Run the secured experience

- Start the Server project (`dotnet run --project src/Server/Server.csproj`).
- Start the Client project (`dotnet run --project src/Client/Client.csproj`).
- Browse to the client URL (default `https://localhost:5002`). You will be redirected to the Azure Entra login flow before protected content loads.

## Tailwind development workflow

The Tailwind CLI is integrated into the build so published assets stay in sync. For interactive development, run the watcher:

```bash
cd src/Client
npm run watch:css
```

Tailwind scans `.razor` and `.html` files for utility classes. The resulting CSS is emitted to `wwwroot/css/app.css`, which is referenced by `wwwroot/index.html`.

## Feature slice architecture overview

- **Client**: feature folders (e.g., `Features/Weather`) contain their components, services, and routes. Shared presentation such as layouts lives under `Layouts/`.
- **Server**: each API capability lives under `Features/<FeatureName>`, combining controllers, handlers, and repositories. Cross-cutting infrastructure (e.g., `Infrastructure/Data`) exposes interfaces consumed by features.
- **Shared**: DTOs and contracts referenced by both client and server are defined once and shared through a class library.

This structure keeps related files together and scales naturally as you add features (auth, profiles, etc.).

## Extending the template

- Add additional features by creating new folders under `src/Client/Features` and `src/Server/Features` with matching DTOs in `src/Shared`.
- Register new feature services in `Program.cs` on both client and server.
- Tailor the Tailwind design tokens in `tailwind.config.js` for your brand while preserving the mobile-first layout.

Enjoy building your Azure Entra-protected mobile-first Blazor experience! ✨
