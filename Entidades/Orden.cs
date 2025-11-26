using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Examen3.Entidades
{
    public class Orden
    {
        [Key]
        public int  Id { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string categoria { get; set; }
        [Required]
        public int calificacion { get; set; }
    }
}
