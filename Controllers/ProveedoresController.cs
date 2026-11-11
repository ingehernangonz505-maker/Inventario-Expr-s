using ControlInventarioExpress.Data;
using ControlInventarioExpress.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlInventarioExpress.Controllers;

public class ProveedoresController(InventarioDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = context.Proveedores.Include(p => p.Productos).AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            if (int.TryParse(buscar.Trim(), out int idBuscado))
            {
                consulta = consulta.Where(p => p.Id == idBuscado);
            }
            else
            {
                consulta = consulta.Where(p => false);
            }
        }

        ViewData["Buscar"] = buscar;
        return View(await consulta.OrderBy(p => p.Id).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return RedirectToAction(nameof(Index));

        var proveedor = await context.Proveedores
            .Include(p => p.Productos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proveedor is null)
        {
            TempData["Error"] = "El proveedor solicitado no existe o fue eliminado.";
            return RedirectToAction(nameof(Index));
        }

        return View(proveedor);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Ruc,Telefono,Correo,Direccion,Estado")] Proveedor proveedor)
    {
        if (!string.IsNullOrWhiteSpace(proveedor.Ruc) &&
            await context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc))
        {
            ModelState.AddModelError(nameof(proveedor.Ruc), "Este RUC ya está registrado.");
        }

        if (!string.IsNullOrWhiteSpace(proveedor.Correo) &&
            await context.Proveedores.AnyAsync(p => p.Correo == proveedor.Correo))
        {
            ModelState.AddModelError(nameof(proveedor.Correo), "Este correo ya está registrado.");
        }

        if (!ModelState.IsValid) return View(proveedor);

        context.Add(proveedor);
        await context.SaveChangesAsync();
        TempData["Success"] = "Proveedor registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return RedirectToAction(nameof(Index));
        var proveedor = await context.Proveedores.FindAsync(id);
        if (proveedor is null)
        {
            TempData["Error"] = "El proveedor solicitado no existe.";
            return RedirectToAction(nameof(Index));
        }
        return View(proveedor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Ruc,Telefono,Correo,Direccion,Estado")] Proveedor proveedor)
    {
        if (id != proveedor.Id) return NotFound();

        if (!string.IsNullOrWhiteSpace(proveedor.Ruc) &&
            await context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc && p.Id != id))
        {
            ModelState.AddModelError(nameof(proveedor.Ruc), "Este RUC pertenece a otro proveedor.");
        }

        if (!string.IsNullOrWhiteSpace(proveedor.Correo) &&
            await context.Proveedores.AnyAsync(p => p.Correo == proveedor.Correo && p.Id != id))
        {
            ModelState.AddModelError(nameof(proveedor.Correo), "Este correo pertenece a otro proveedor.");
        }

        if (!ModelState.IsValid) return View(proveedor);

        context.Update(proveedor);
        await context.SaveChangesAsync();
        TempData["Success"] = "Proveedor actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return RedirectToAction(nameof(Index));
        var proveedor = await context.Proveedores.FirstOrDefaultAsync(p => p.Id == id);
        if (proveedor is null)
        {
            TempData["Error"] = "El proveedor solicitado no existe.";
            return RedirectToAction(nameof(Index));
        }
        return View(proveedor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proveedor = await context.Proveedores
            .Include(p => p.Productos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proveedor is null) return RedirectToAction(nameof(Index));

        if (proveedor.Productos.Count > 0)
        {
            TempData["Error"] = "No se puede eliminar este proveedor porque tiene productos asociados.";
            return RedirectToAction(nameof(Index));
        }

        context.Proveedores.Remove(proveedor);
        await context.SaveChangesAsync();
        TempData["Success"] = "Proveedor eliminado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
