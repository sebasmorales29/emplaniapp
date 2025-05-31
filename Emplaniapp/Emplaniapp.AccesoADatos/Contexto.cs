using Emplaniapp.Abstracciones.ModelosAD;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emplaniapp.AccesoADatos
{
    public class Contexto : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoRemuneracion>().ToTable("TipoRemuneracion");
            modelBuilder.Entity<TipoRetencion>().ToTable("TipoRetencion");
        }


        // Entidades ---------------------------------------
        public DbSet<TipoRemuneracion> TipoRemu {  get; set; }
        public DbSet<TipoRetencion> TipoReten {  get; set; }


    }
}
