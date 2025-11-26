using System.ComponentModel.DataAnnotations;

namespace Examen3.Entidades
{
    public class Compras
    {
        [Key]
        public int Codigo { get; set; }
        [Required]
        public int ProveedorCodigo { get; set; }
        [Required]
        public DateOnly Fecha { get; set; }
        [Required]
        public string Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal CostoTotal { get; set; }
    }
}
