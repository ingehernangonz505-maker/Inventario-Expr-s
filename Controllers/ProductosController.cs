using ControlInventarioExpress.Data;
using ControlInventarioExpress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ControlInventarioExpress.Controllers;

public class ProductosController(InventarioDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = context.Productos.Include(p => p.Proveedor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            consulta = consulta.Where(p => p.Nombre.Contains(buscar) ||
                                           (p.Categoria != null && p.Categoria.Contains(buscar)) ||
                                           (p.Proveedor != null && p.Proveedor.Nombre.Contains(buscar)));
        }

        ViewData["Buscar"] = buscar;
        return View(await consulta.OrderBy(p => p.Nombre).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var producto = await context.Productos.Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);
        return producto is null ? NotFound() : View(producto);
    }

    public async Task<IActionResult> Create()
    {
        await CargarProveedores();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Categoria,Descripcion,PrecioCompra,PrecioVenta,Stock,FechaVencimiento,ProveedorId")] Producto producto)
    {
        if (producto.PrecioVenta < producto.PrecioCompra)
        {
            ModelState.AddModelError(nameof(producto.PrecioVenta), "El precio de venta no puede ser menor al precio de compra.");
        }

        if (!await context.Proveedores.AnyAsync(p => p.Id == producto.ProveedorId))
        {
            ModelState.AddModelError(nameof(producto.ProveedorId), "El proveedor seleccionado no existe.");
        }

        if (!ModelState.IsValid)
        {
            await CargarProveedores(producto.ProveedorId);
            return View(producto);
        }

        context.Add(producto);
        await context.SaveChangesAsync();
        TempData["Success"] = "Producto registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var producto = await context.Productos.FindAsync(id);
        if (producto is null) return NotFound();
        await CargarProveedores(producto.ProveedorId);
        return View(producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Categoria,Descripcion,PrecioCompra,PrecioVenta,Stock,FechaVencimiento,ProveedorId")] Producto producto)
    {
        if (id != producto.Id) return NotFound();

        if (producto.PrecioVenta < producto.PrecioCompra)
        {
            ModelState.AddModelError(nameof(producto.PrecioVenta), "El precio de venta no puede ser menor al precio de compra.");
        }

        if (!await context.Proveedores.AnyAsync(p => p.Id == producto.ProveedorId))
        {
            ModelState.AddModelError(nameof(producto.ProveedorId), "El proveedor seleccionado no existe.");
        }

        if (!ModelState.IsValid)
        {
            await CargarProveedores(producto.ProveedorId);
            return View(producto);
        }

        try
        {
            context.Update(producto);
            await context.SaveChangesAsync();
            TempData["Success"] = "Producto actualizado correctamente.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await context.Productos.AnyAsync(p => p.Id == id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var producto = await context.Productos.Include(p => p.Proveedor)
            .FirstOrDefaultAsync(p => p.Id == id);
        return producto is null ? NotFound() : View(producto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await context.Productos.FindAsync(id);
        if (producto is not null)
        {
            context.Productos.Remove(producto);
            await context.SaveChangesAsync();
            TempData["Success"] = "Producto eliminado correctamente.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarProveedores(int? seleccionado = null)
    {
        ViewData["ProveedorId"] = new SelectList(
            await context.Proveedores.Where(p => p.Estado).OrderBy(p => p.Nombre).ToListAsync(),
            "Id", "Nombre", seleccionado);
    }
}
