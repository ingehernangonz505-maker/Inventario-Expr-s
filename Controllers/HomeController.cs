using ControlInventarioExpress.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlInventarioExpress.Controllers;

public class HomeController(InventarioDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var productos = await context.Productos.Include(p => p.Proveedor).ToListAsync();
        ViewBag.TotalProductos = productos.Count;
        ViewBag.TotalProveedores = await context.Proveedores.CountAsync();
        ViewBag.ValorInventario = productos.Sum(p => p.PrecioVenta * p.Stock);

        return View(productos);
    }
}
