# Control de Inventario Exprés

Aplicación web ASP.NET Core MVC 10 con C#, Entity Framework Core 10 y SQL Server local.

## Requisitos
- .NET 10 SDK
- SQL Server local (instancia `localhost` o ajustar la cadena de conexión)
- Visual Studio 2026 o Visual Studio Code

## Configuración
1. Abre `ControlInventarioExpress.csproj` en Visual Studio o abre esta carpeta en una terminal.
2. Revisa `appsettings.json`. La conexión predeterminada usa:
   `Server=localhost;Database=InventarioExpress;Trusted_Connection=True;TrustServerCertificate=True;`
3. Si usas una instancia SQL Express, cambia `Server=localhost` por `Server=localhost\\SQLEXPRESS`.
4. En la terminal, dentro de la carpeta del proyecto, ejecuta:

```powershell
dotnet restore
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

5. Abre la URL HTTPS que muestre la terminal.

## Funcionalidades
- Dashboard con métricas de inventario.
- CRUD de proveedores.
- CRUD de productos.
- Selección de proveedor al registrar/editar productos.
- Búsqueda por nombre de producto o proveedor.
- Validaciones de datos y protección de eliminación de proveedores con productos asociados.

## Nota sobre la base de datos
Este proyecto está preparado para crear la base `InventarioExpress` y sus tablas mediante migraciones de EF Core. No ejecutes además un script que cree las mismas tablas manualmente. Si ya tienes una base con esas tablas, haz una copia de seguridad y revisa el esquema antes de aplicar migraciones.
