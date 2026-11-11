using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlInventarioExpress.Models;

[Table("proveedor")]
public class Proveedor
{
    [Key]
    [Column("idproveedor")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [StringLength(100)]
    [Column("nombreproveedor")]
    [Display(Name = "Nombre del Proveedor")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20)]
    [Column("ruc")]
    [Display(Name = "RUC")]
    public string? Ruc { get; set; }

    [StringLength(20)]
    [Column("telefono")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
    [StringLength(100)]
    [Column("correo")]
    [Display(Name = "Correo Electrónico")]
    public string? Correo { get; set; }

    [StringLength(200)]
    [Column("direccion")]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }

    [Column("estado")]
    [Display(Name = "Activo")]
    public bool Estado { get; set; } = true;

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
