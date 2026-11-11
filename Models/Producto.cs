using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlInventarioExpress.Models;

[Table("producto")]
public class Producto
{
    [Key]
    [Column("idproducto")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [StringLength(100)]
    [Column("nombreproducto")]
    [Display(Name = "Producto")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(50)]
    [Column("categoria")]
    [Display(Name = "Categoría")]
    public string? Categoria { get; set; }

    [StringLength(250)]
    [Column("descripcion")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio de compra es obligatorio.")]
    [Range(0, 999999999, ErrorMessage = "El precio de compra no puede ser negativo.")]
    [Column("preciocompra", TypeName = "decimal(10,2)")]
    [Display(Name = "Precio de Compra")]
    public decimal PrecioCompra { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio.")]
    [Range(0, 999999999, ErrorMessage = "El precio de venta no puede ser negativo.")]
    [Column("precioventa", TypeName = "decimal(10,2)")]
    [Display(Name = "Precio de Venta")]
    public decimal PrecioVenta { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    [Column("stock")]
    [Display(Name = "Stock")]
    public int Stock { get; set; }

    [DataType(DataType.Date)]
    [Column("fechavencimiento")]
    [Display(Name = "Fecha de Vencimiento")]
    public DateTime? FechaVencimiento { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un proveedor válido.")]
    [Column("idproveedor")]
    [Display(Name = "Proveedor")]
    public int ProveedorId { get; set; }

    [ForeignKey("ProveedorId")]
    public Proveedor? Proveedor { get; set; }
}
