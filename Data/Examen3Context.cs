using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Examen3.Entidades;

namespace Examen3.Data
{
    public class Examen3Context : DbContext
    {
        public Examen3Context (DbContextOptions<Examen3Context> options)
            : base(options)
        {
        }

        public DbSet<Examen3.Entidades.Compras> Compras { get; set; } = default!;
        public DbSet<Examen3.Entidades.Orden> Orden { get; set; } = default!;
    }
}
