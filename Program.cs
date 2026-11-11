using ControlInventarioExpress.Data;
using ControlInventarioExpress.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Base de datos en memoria para ver el frontend al instante
builder.Services.AddDbContext<InventarioDbContext>(options =>
    options.UseInMemoryDatabase("VistaFrontend"));

var app = builder.Build();

// Precarga automática de datos iniciales en memoria (Tus datos intactos)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InventarioDbContext>();
    context.Database.EnsureCreated();

    if (!context.Proveedores.Any())
    {
        context.Proveedores.AddRange(
            new Proveedor { Nombre = "Distribuidora Veterinaria Central", Ruc = "001-150890-1020A", Telefono = "8881-2233", Correo = "ventas@vetcentral.com", Direccion = "Managua", Estado = true },
            new Proveedor { Nombre = "Agrovet Nicaragua", Ruc = "001-220490-1021B", Telefono = "8772-3344", Correo = "contacto@agrovet.com", Direccion = "Masaya", Estado = true },
            new Proveedor { Nombre = "Mundo Animal Distribuciones", Ruc = "001-180385-1022C", Telefono = "8663-4455", Correo = "ventas@mundoanimal.com", Direccion = "Granada", Estado = true },
            new Proveedor { Nombre = "Comercial Pet Supplies", Ruc = "001-050790-1023D", Telefono = "8554-5566", Correo = "pedidos@petsupplies.com", Direccion = "Jinotepe", Estado = true },
            new Proveedor { Nombre = "Farmacia Veterinaria San Jose", Ruc = "001-120688-1024E", Telefono = "8445-6677", Correo = "info@farmavet.com", Direccion = "Managua", Estado = true },
            new Proveedor { Nombre = "Distribuidora El Ganadero", Ruc = "001-100580-1025F", Telefono = "8336-7788", Correo = "ventas@elganadero.com", Direccion = "Masatepe", Estado = true },
            new Proveedor { Nombre = "Productos Veterinarios del Norte", Ruc = "001-140790-1026G", Telefono = "8227-8899", Correo = "ventas@vetnorte.com", Direccion = "Esteli", Estado = true },
            new Proveedor { Nombre = "Alimentos para Mascotas Nic", Ruc = "001-170895-1027H", Telefono = "8118-9900", Correo = "pedidos@mascotasnic.com", Direccion = "Managua", Estado = true },
            new Proveedor { Nombre = "Laboratorios Veterinarios Unidos", Ruc = "001-190688-1028I", Telefono = "8991-1122", Correo = "contacto@labvet.com", Direccion = "Leon", Estado = true },
            new Proveedor { Nombre = "Accesorios Mundo Pet", Ruc = "001-210990-1029J", Telefono = "8882-3344", Correo = "ventas@mundopet.com", Direccion = "Masaya", Estado = true }
        );
        context.SaveChanges();

        context.Productos.AddRange(
            new Producto { Nombre = "Vitaminas Pet", Categoria = "Vitaminas", Descripcion = "Suplemento vitaminico para perros y gatos", PrecioCompra = 80.00m, PrecioVenta = 120.00m, Stock = 35, FechaVencimiento = new DateTime(2026, 8, 15), ProveedorId = 1 },
            new Producto { Nombre = "Amoxicilina 250mg", Categoria = "Medicamentos", Descripcion = "Antibiotico veterinario", PrecioCompra = 100.00m, PrecioVenta = 150.00m, Stock = 20, FechaVencimiento = new DateTime(2026, 5, 20), ProveedorId = 5 },
            new Producto { Nombre = "Shampoo Medicado", Categoria = "Higiene", Descripcion = "Shampoo para problemas de piel", PrecioCompra = 90.00m, PrecioVenta = 135.00m, Stock = 25, FechaVencimiento = new DateTime(2026, 1, 10), ProveedorId = 2 },
            new Producto { Nombre = "Alimento Dog Chow 2kg", Categoria = "Alimentos", Descripcion = "Comida para perros adultos", PrecioCompra = 180.00m, PrecioVenta = 230.00m, Stock = 40, FechaVencimiento = new DateTime(2026, 12, 10), ProveedorId = 8 },
            new Producto { Nombre = "Collar Antipulgas", Categoria = "Accesorios", Descripcion = "Collar para control de pulgas", PrecioCompra = 150.00m, PrecioVenta = 210.00m, Stock = 15, FechaVencimiento = null, ProveedorId = 10 }
        );
        context.SaveChanges();
    }
}

// Muestra siempre el detalle técnico si ocurre algún fallo al probar el CRUD
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

